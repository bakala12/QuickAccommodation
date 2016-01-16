using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult ChangeUserData()
        {
            ViewBag.Title = "Edytuj swoje dane";
            return View();
        }

        [HttpPost]
        public ActionResult ChangeUserData(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                   
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