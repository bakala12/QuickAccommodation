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
using AccomodationWebApi.Providers;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

namespace AccommodationWebPage.Controllers
{
    /// <summary>
    /// Odpowiada za logowanie i rejestracje użytkowników.
    /// </summary>
    public class AccountController : AccommodationController
    {
        /// <summary>
        /// Inicjalizuje nową instancję kontrolera
        /// </summary>
        /// <param name="provider">Dosawca kontekstu bazy danych</param>
        public AccountController(IContextProvider provider) : base(provider) { }

        /// <summary>
        /// Inicajlizuje nową instancję kontrolera.
        /// </summary>
        public AccountController() : base(new ContextProvider<AccommodationContext>()) { }
        
        /// <summary>
        /// Zwraca widok logowania dla użytkownika.
        /// </summary>
        /// <param name="returnUrl">Url na które zostanie przekierowany użytkownik.</param>
        /// <returns>Widok logowania.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Loguje użytkownika na stronie.
        /// </summary>
        /// <param name="model">Model z danymi logowania użytkownika.</param>
        /// <param name="returnUrl">Url na który zostanie przekierowany użytkownik.</param>
        /// <returns>Widok logowania jeśli logowanie przebiegło błędnie, jeśli przebiegło poprawnie to 
        /// następuje przekierowanie do Home/Index</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result=Authenticator.Instance.SignIn(Context, model.Username, model.Password);
            if (result != null)
            {
                Response.SetCookie(new HttpCookie("auth", result.Token));
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło");
            return View(model);
        }

        /// <summary>
        /// Wylogowje użytkownika z aplikacji.
        /// </summary>
        /// <returns>Przekierowuje uzytkownika do strony Home/Index</returns>
        [HttpPost]
        [AuthorizationRequired]
        public ActionResult LogOff()
        {
            Response.SetCookie(new HttpCookie("auth") { Expires = DateTime.Now.AddDays(-1) }); //ciastko wygasa
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Zwraca widok rejestracji nowego użytkownika.
        /// </summary>
        /// <returns>Widok rejestracji nowego użytkownika.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Title = "Rejestracja";
            return View();
        }

        /// <summary>
        /// Rejestruje nowego użytkownika w aplikacji.
        /// </summary>
        /// <param name="model">Model z danymi użytkownika.</param>
        /// <returns>Jeśli rejestracja przebiegła błędnie zwraca widok rejestracji, jeśli
        /// przebiegła pomyślnie przekierowuje do strony Home/Index</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string error = await Registration.Instance.ValidateUserAsync(Context, model);
                if (!string.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("", error);
                    return View(model);
                }
                await Registration.Instance.SaveUserAsync(Context, model);
                var result = Authenticator.Instance.SignIn(Context, model.Username, model.Password);
                if (result != null)
                    return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        /// <summary>
        /// Zwraca widok zabronienego dostępu dla niezalogowanego użytkownika.
        /// </summary>
        /// <returns>Widok zabronienego dostępu dla niezalogowango użytkownika.</returns>
        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }

        #region PrivateHelpers
        /// <summary>
        /// Przekierowuje użytkownika do podanego lokalnego adresu lub do Home/Index.
        /// </summary>
        /// <param name="returnUrl">Adres lokalny do przekierowania.</param>
        /// <returns>Przekierowanie do adresu lu Home/Index.</returns>
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