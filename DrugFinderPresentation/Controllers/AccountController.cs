using DrugFinderMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace DrugFinderMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
<<<<<<< HEAD
=======
            // If already properly authenticated redirect away
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

>>>>>>> origin/Mostafa
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("DrugFinderAPI");

            var response = await client.GetAsync(
                $"api/Account/Login?email={Uri.EscapeDataString(model.Email)}&password={Uri.EscapeDataString(model.Password)}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var token = await response.Content.ReadAsStringAsync();
<<<<<<< HEAD
            token = token.Trim('"'); // remove surrounding quotes from JSON string

            // Store token in session
            HttpContext.Session.SetString("JwtToken", token);

            // Parse JWT to get claims for cookie auth
            var parts = token.Split('.');
            if (parts.Length == 3)
            {
                var payload = parts[1];
                // pad base64
                payload += new string('=', (4 - payload.Length % 4) % 4);
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                var claims = new List<Claim> { new Claim("JwtToken", token) };

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("email", out var emailProp))
                    claims.Add(new Claim(ClaimTypes.Email, emailProp.GetString() ?? ""));

                if (root.TryGetProperty("nameid", out var idProp))
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, idProp.GetString() ?? ""));

                // roles can be a string or array
                if (root.TryGetProperty("role", out var roleProp))
                {
                    if (roleProp.ValueKind == JsonValueKind.Array)
                        foreach (var r in roleProp.EnumerateArray())
                            claims.Add(new Claim(ClaimTypes.Role, r.GetString() ?? ""));
                    else
                        claims.Add(new Claim(ClaimTypes.Role, roleProp.GetString() ?? ""));
                }

                // Also check http://schemas.microsoft.com/ws/2008/06/identity/claims/role
                if (root.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var roleProp2))
                {
                    if (roleProp2.ValueKind == JsonValueKind.Array)
                        foreach (var r in roleProp2.EnumerateArray())
                            claims.Add(new Claim(ClaimTypes.Role, r.GetString() ?? ""));
                    else
                        claims.Add(new Claim(ClaimTypes.Role, roleProp2.GetString() ?? ""));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

=======
            token = token.Trim('"');

            // Store token in session for use by API calls
            HttpContext.Session.SetString("JwtToken", token);

            // Parse JWT payload to build claims principal
            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                ModelState.AddModelError("", "Invalid token received from server.");
                return View(model);
            }

            var payload = parts[1];
            payload += new string('=', (4 - payload.Length % 4) % 4);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Hash, token)
            };

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Email claim (ASP.NET Identity uses the long URI form)
            if (root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", out var ep))
                claims.Add(new Claim(ClaimTypes.Email, ep.GetString() ?? ""));
            else if (root.TryGetProperty("email", out var ep2))
                claims.Add(new Claim(ClaimTypes.Email, ep2.GetString() ?? ""));

            // NameIdentifier
            if (root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out var np))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, np.GetString() ?? ""));
            else if (root.TryGetProperty("nameid", out var np2))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, np2.GetString() ?? ""));

            // Roles — try both short and long claim URI forms
            void AddRoleClaims(JsonElement elem)
            {
                if (elem.ValueKind == JsonValueKind.Array)
                    foreach (var r in elem.EnumerateArray())
                        claims.Add(new Claim(ClaimTypes.Role, r.GetString() ?? ""));
                else
                    claims.Add(new Claim(ClaimTypes.Role, elem.GetString() ?? ""));
            }

            if (root.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var rp))
                AddRoleClaims(rp);
            else if (root.TryGetProperty("role", out var rp2))
                AddRoleClaims(rp2);

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // IsPersistent = true writes a real expiry on the cookie so
            // it cannot survive beyond ExpireTimeSpan even after a browser restart.
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc   = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProps);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // use principal (NOT User)
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (principal.IsInRole("SuperAdmin"))
                return RedirectToAction("Index", "Admin");

            if (principal.IsInRole("Pharmacy"))
                return RedirectToAction("Index", "Pharmacy");

            if (principal.IsInRole("Patient"))
                return RedirectToAction("Index", "Patient");

>>>>>>> origin/Mostafa
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
<<<<<<< HEAD
=======
            HttpContext.Session.Clear();
>>>>>>> origin/Mostafa
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
