using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Providers;

namespace AccommodationWebPage.Controllers
{
    public class OfferController : AccommodationController
    {
        public OfferController(IContextProvider provider) : base(provider) { }

        public OfferController() : base(new ContextProvider<AccommodationContext>()) { }

        // GET: /OfferWorld/ 
        public ActionResult Index()
        {
            return View();
        }

        // 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}