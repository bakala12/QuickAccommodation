using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccommodationWebPage.Authorization;
using AccommodationWebPage.Models;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;

namespace AccommodationWebPage.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //zalogowany nie może tego zrobić
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            IUserAuthenticationService authenticationService = new UserAuthenticationService();
            CustomIdentity identity =
                await authenticationService.AuthenticateUserAsync<AccommodationContext>(model.Username, model.Password);
            if (identity != null)
            {
                Authorization.Authorization.Current.Login(identity);
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorization]
        public ActionResult LogOff()
        {
            Authorization.Authorization.Current.Logout();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
            return View();
        }

        [RequireAuthorization]
        public string ViewProfile()
        {
            return "Twój profil";
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}