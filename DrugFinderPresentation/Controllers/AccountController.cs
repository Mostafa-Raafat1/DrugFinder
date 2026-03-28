using DrugFinderMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
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
            token = token.Trim('"');

            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                ModelState.AddModelError("", "Invalid token received from server.");
                return View(model);
            }

            var payload = parts[1];
            payload += new string('=', (4 - payload.Length % 4) % 4);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));

            var claims = new List<Claim> { new Claim("JwtToken", token) };

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Email
            if (root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", out var ep))
                claims.Add(new Claim(ClaimTypes.Email, ep.GetString() ?? ""));
            else if (root.TryGetProperty("email", out var ep2))
                claims.Add(new Claim(ClaimTypes.Email, ep2.GetString() ?? ""));

            // NameIdentifier
            if (root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out var np))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, np.GetString() ?? ""));
            else if (root.TryGetProperty("nameid", out var np2))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, np2.GetString() ?? ""));

            // Roles
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

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProps);

            // ✅ Must come after SignInAsync so the session is initialised
            HttpContext.Session.SetString("JwtToken", token);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (principal.IsInRole("SuperAdmin"))
                return RedirectToAction("Index", "Admin");

            if (principal.IsInRole("Pharmacy"))
                return RedirectToAction("Index", "Pharmacy");

            if (principal.IsInRole("Patient"))
                return RedirectToAction("Index", "Patient");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}