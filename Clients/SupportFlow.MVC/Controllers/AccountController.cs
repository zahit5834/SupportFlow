using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SupportFlow.Auth.Business.Dtos;
using System.Security.Claims;

namespace SupportFlow.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var response = await client.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

                // Cookie içine Token'ı gömüyoruz (İleride Claims ekleyebiliriz)
                var claims = new List<Claim> { new Claim("Token", result.AccessToken) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "E-posta veya şifre hatalı!");
            return View();
        }
    }
}
