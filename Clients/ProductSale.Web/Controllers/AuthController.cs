using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;
using ProductSale.Web.Models;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput signInInput)
        {
            if (ModelState.IsValid is false)
            {
                return View(signInInput);
            }

            Response response = await _identityService.SignIn(signInInput);

            if (response.Success is false)
            {
                ModelState.AddModelError("", ((ErrorResponse)response).Errors.FirstOrDefault());

                return View(signInInput);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SignOut(SignInInput signInInput)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _identityService.RevokeRefreshToken();
            return RedirectToAction("Index", "Home");
        }
    }
}
