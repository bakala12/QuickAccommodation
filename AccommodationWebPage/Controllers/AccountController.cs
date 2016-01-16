using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationWebPage.Authorization;
using AccommodationWebPage.Models;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

namespace AccommodationWebPage.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result=Authenticator.Instance.SignIn(model.Username, model.Password);
            if (result != null)
            {
                Response.SetCookie(new HttpCookie("auth", result.Token));
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło");
            return View(model);
        }

        [HttpPost]
        [AuthorizationRequired]
        public ActionResult LogOff()
        {
            Response.SetCookie(new HttpCookie("auth") { Expires = DateTime.Now.AddDays(-1) }); //ciastko wygasa
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Title = "Rejestracja";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string error = await Registration.Instance.ValidateUserAsync(model);
                if (!string.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("", error);
                    return View(model);
                }
                await Registration.Instance.SaveUserAsync(model);
                var result = Authenticator.Instance.SignIn(model.Username, model.Password);
                if(result!=null)
                    return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        [AuthorizationRequired]
        public ActionResult ViewProfile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }

        #region PrivateHelpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}