<<<<<<< Updated upstream
namespace DrugFinderPresentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
=======
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// HttpClient for API calls
builder.Services.AddHttpClient("DrugFinderAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5080");
});

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath     = "/Account/Login";
        options.LogoutPath    = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        // Sliding expiration: each request within the hour resets the timer
        options.SlidingExpiration = true;
        // Validate the JWT claim is present on every request;
        // if the cookie is stale (e.g. server restarted, session cleared)
        // the user is signed out automatically instead of seeing phantom auth state.
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async ctx =>
            {
                var token = ctx.Principal?.FindFirstValue(ClaimTypes.Hash);
                if (string.IsNullOrEmpty(token))
                {
                    // Cookie exists but JWT is missing — reject it
                    ctx.RejectPrincipal();
                    await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return;
                }

                // Check the JWT expiry without calling the API
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwt = handler.ReadJwtToken(token);
                        if (jwt.ValidTo < DateTime.UtcNow)
                        {
                            // Token expired — sign out
                            ctx.RejectPrincipal();
                            await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        }
                    }
                }
                catch
                {
                    ctx.RejectPrincipal();
                    await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        };
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout     = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly  = true;
    options.Cookie.IsEssential = true;
});
>>>>>>> Stashed changes

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
