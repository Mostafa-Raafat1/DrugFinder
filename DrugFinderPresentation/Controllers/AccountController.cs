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

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
