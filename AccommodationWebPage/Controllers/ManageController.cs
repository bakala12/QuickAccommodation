using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccommodationWebPage.Authorization;
using AccommodationWebPage.DataAccess;
using AccommodationWebPage.Models;
using AccomodationWebApi.Providers;

namespace AccommodationWebPage.Controllers
{
    /// <summary>
    /// Kontroler do zarządzania kontem użytkownika.
    /// Operacje z tego kontrolera wymagają aby użytkownik był zalogowany.
    /// </summary>
    [AuthorizationRequired]
    public class ManageController : AccommodationController
    {
        /// <summary>
        /// Tworzy nową instancję kontrolera uzywając podanego obiektu IContextProvider.
        /// </summary>
        /// <param name="provider"></param>
        public ManageController(IContextProvider provider) : base(provider) { }

        /// <summary>
        /// Tworzy nową instancję z domyślnym połączeniem do bazy danych AccommodationContext.
        /// </summary>
        public ManageController() : base(new ContextProvider<AccommodationContext>()) { }

        private readonly UserDataAccessor _userDataAccessor = new UserDataAccessor();

        /// <summary>
        /// Wyświetla widok profilu użytkownika.
        /// </summary>
        /// <returns>Widok profilu użytkownika.</returns>
        [HttpGet]
        public ActionResult ViewProfile()
        {
            if (string.IsNullOrEmpty(HttpContext.User.Identity?.Name))
                return RedirectToAction("AccessDenied", "Account");
            var model = _userDataAccessor.GetInfoAboutUser(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

        /// <summary>
        /// Zwraca widok do zmiany danych uzytkownika.
        /// </summary>
        /// <returns>Widok do zmiany danych użytkownika.</returns>
        [HttpGet]
        public async Task<ActionResult> ChangeUserData()
        {
            ViewBag.Title = "Edytuj swoje dane";
            var model = await _userDataAccessor.GetInfoAboutUserAsync(Context, HttpContext.User?.Identity?.Name);
            return View(new ChangeUserDataViewModel(model));
        }

        /// <summary>
        /// Zmienia dane użytkownika.
        /// </summary>
        /// <param name="model">Model zawierający nowe dane uzytkownika.</param>
        /// <returns>Widok zmiany danych jeśli dane są niepoprawne lub przekierowanie do zarządzania profilem.</returns>
        [HttpPost]
        public async Task<ActionResult> ChangeUserData(ChangeUserDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (! await _userDataAccessor.SaveUserDataAsync(Context, HttpContext.User?.Identity?.Name, model))
                {
                    ModelState.AddModelError("", "Nie udało się zapisać nowych danych");
                }
                return RedirectToAction("ViewProfile", "Manage");
            }
            return View(model);
        }

        /// <summary>
        /// Wyświetla widok do zmiany hasła.
        /// </summary>
        /// <returns>Widok do zmiany haśła</returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewBag.Title = "Zmiana hasła";
            return View();
        }

        /// <summary>
        /// Zmienia hasło użytkownika.
        /// </summary>
        /// <param name="model">Model z nowym hasłem</param>
        /// <returns>Widok do zmiany hasła lub przekierowanie do zarządzania profilem.</returns>
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string s = await _userDataAccessor.ChangePasswordAsync(Context, HttpContext.User?.Identity?.Name, model);
                if (string.IsNullOrEmpty(s))
                {
                    return RedirectToAction("ViewProfile", "Manage");
                }
                ModelState.AddModelError("", s);
            }
            return View();
        }
    }
}