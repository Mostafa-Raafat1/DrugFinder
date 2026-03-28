using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DrugFinderPresentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient("DrugFinderAPI", client =>
            {
                client.BaseAddress = new Uri(
                    builder.Configuration["ApiSettings:BaseUrl"]
                    ?? "http://localhost:5080");
            });

            // ✅ Session before Authentication
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.SlidingExpiration = true;

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = async ctx =>
                        {
                            // Skip validation on login/logout to prevent loops
                            var path = ctx.HttpContext.Request.Path;
                            if (path.StartsWithSegments("/Account/Login") ||
                                path.StartsWithSegments("/Account/Logout"))
                                return;

                            var token = ctx.Principal?.FindFirstValue("JwtToken");

                            if (string.IsNullOrEmpty(token))
                            {
                                ctx.RejectPrincipal();
                                await ctx.HttpContext.SignOutAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme);
                                return;
                            }

                            try
                            {
                                var handler = new JwtSecurityTokenHandler();

                                if (!handler.CanReadToken(token))
                                {
                                    ctx.RejectPrincipal();
                                    await ctx.HttpContext.SignOutAsync(
                                        CookieAuthenticationDefaults.AuthenticationScheme);
                                    return;
                                }

                                var jwt = handler.ReadJwtToken(token);

                                if (jwt.ValidTo < DateTime.UtcNow)
                                {
                                    ctx.RejectPrincipal();
                                    await ctx.HttpContext.SignOutAsync(
                                        CookieAuthenticationDefaults.AuthenticationScheme);
                                    return;
                                }

                                // ✅ Key fix: restore JWT into session from
                                // the cookie claim after every restart
                                var sessionToken = ctx.HttpContext.Session
                                                      .GetString("JwtToken");
                                if (string.IsNullOrEmpty(sessionToken))
                                    ctx.HttpContext.Session.SetString("JwtToken", token);
                            }
                            catch
                            {
                                ctx.RejectPrincipal();
                                await ctx.HttpContext.SignOutAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme);
                            }
                        }
                    };
                });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}