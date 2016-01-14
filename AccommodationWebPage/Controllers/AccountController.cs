using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccommodationWebPage.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ViewProfile()
        {
            return View();
        }
    }
}