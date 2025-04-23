using JoygameProject.Application.Features.Commands.Login;
using JoygameProject.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JoygameProject.Web.Controllers
{
    [Route("[action]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Login()
        {
            var model = new LoginViewModel();

            if (Request.Cookies.TryGetValue("rememberedEmail", out var email))
            {
                model.Email = email;
                model.RememberMe = true;
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordRequest(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("ForgotPassword", model);

            var result = await _mediator.Send(new ForgotPasswordCommandRequest() { Email = model.Email });

            if (result.Status != HttpStatusCode.OK)
            {
                ModelState.AddModelError("", result.Message ?? "Bir hata oluştu.");
                return View(model);
            }

            TempData["Success"] = "Şifre sıfırlama maili gönderildi.";
            return RedirectToAction("Login");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _mediator.Send(new LoginCommandRequest
            {
                Email = model.Email,
                Password = model.Password
            });

            if (response.Status != HttpStatusCode.OK)
            {
                ModelState.AddModelError("", response.Message ?? "Giriş başarısız");
                return View(model);
            }

            HttpContext.Response.Cookies.Append("jwt", response.Result!.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            if (model.RememberMe)
            {
                Response.Cookies.Append("rememberedEmail", model.Email, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
            }
            else
            {
                Response.Cookies.Delete("rememberedEmail");
            }
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Redirect("/");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var command = new ResetPasswordCommandRequest
            {
                Token = model.Token,
                NewPassword = model.NewPassword
            };

            var result = await _mediator.Send(command);

            if (!result.Result)
            {
                ModelState.AddModelError("", result.Message ?? "Bir hata oluştu");
                return View(model);
            }

            return RedirectToAction("Login", "Auth");
        }
    }
}
