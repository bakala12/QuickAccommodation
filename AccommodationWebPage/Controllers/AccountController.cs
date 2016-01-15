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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Title = "Rejestracja";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string error = null;
                IUserCredentialsValidator validator = new UserCredentialsValidator();
                if (!await validator.ValidateUsernameAsync<AccommodationContext>(model.Username))
                {
                    error = "Podana nazwa użytkownika już istnieje. Należy podać unikalną nazwę użytkownika.";
                    ModelState.AddModelError("", error);
                    return View(model);
                }
                if (!ValidateUserData(model, out error))
                {
                    ModelState.AddModelError("", error);
                    return View(model);
                }
                IRegisterUser registerService = new UserRegister();
                User user = registerService.GetNewUser(model.Username, model.Password);
                UserData data = new UserData()
                {
                    Email = model.Email,
                    CompanyName = model.CompanyName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                Address address = new Address()
                {
                    City = model.City,
                    Street = model.Street,
                    LocalNumber = model.LocalNumber,
                    PostalCode = model.PostalCode
                };
                await registerService.SaveUserAsync<AccommodationContext>(user, data, address);
                IUserAuthenticationService authenticationService = new UserAuthenticationService();
                CustomIdentity identity = await authenticationService.AuthenticateUserAsync<AccommodationContext>(model.Username,model.Password);
                if (identity != null)
                {
                    Authorization.Authorization.Current.Login(identity);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        [RequireAuthorization]
        public string ViewProfile()
        {
            return "Twój profil";
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

        private bool ValidateUserData(RegisterViewModel model, out string errorMessage)
        {
            IUserCredentialsValidator validator = new UserCredentialsValidator();
            if (!validator.ValidateLocalNumber(model.LocalNumber))
            {
                errorMessage = "Numer domu musi zaczynać się cyfrą";
                return false;
            }
            if (!validator.ValidatePostalCode(model.PostalCode))
            {
                errorMessage = "Nieprawidłowy kod pocztowy";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        #endregion
    }
}