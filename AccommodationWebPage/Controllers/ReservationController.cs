using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Providers;
using AccommodationWebPage.Models;
using AccommodationWebPage.Authorization;
using AccommodationWebPage.Validation;
using AccommodationWebPage.DataAccess;
using System.Threading.Tasks;

namespace AccommodationWebPage.Controllers
{
    [AuthorizationRequired]
    public class ReservationController : AccommodationController
    {
        public ReservationController(IContextProvider provider) : base(provider)
        {
        }

        public ReservationController() : base(new ContextProvider<AccommodationContext>())
        {
        }

        public async Task<ActionResult> ReserveOffer(int id)
        {
            string username = HttpContext.User?.Identity?.Name;
            if (OfferAccessor.ReserveOffer(Context, id, username))
            {
                return RedirectToAction("Done", "Reservation");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<ActionResult> ResignOffer(int id)
        {
            string username = HttpContext.User?.Identity?.Name;
            if (await OfferAccessor.ResignOfferAsync(Context, id, username))
            {
                return RedirectToAction("Done", "Reservation");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Done()
        {
            return View();
        }

    }
}