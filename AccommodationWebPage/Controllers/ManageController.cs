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
    [AuthorizationRequired]
    public class ManageController : AccommodationController
    {
        public ManageController(IContextProvider provider) : base(provider) { }

        public ManageController() : base(new ContextProvider<AccommodationContext>()) { }

        private readonly UserDataAccessor _userDataAccessor = new UserDataAccessor();

        [HttpGet]
        public ActionResult ViewProfile()
        {
            if (string.IsNullOrEmpty(HttpContext.User.Identity?.Name))
                return RedirectToAction("AccessDenied", "Account");
            var model = _userDataAccessor.GetInfoAboutUser(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ChangeUserData()
        {
            ViewBag.Title = "Edytuj swoje dane";
            var model = await _userDataAccessor.GetInfoAboutUserAsync(Context, HttpContext.User?.Identity?.Name);
            return View(new ChangeUserDataViewModel(model));
        }

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

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}