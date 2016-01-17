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
    public class OfferController : AccommodationController
    {
        public OfferController(IContextProvider provider) : base(provider) { }

        public OfferController() : base(new ContextProvider<AccommodationContext>()) { }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> MyOffers()
        {
            string username = HttpContext.User?.Identity?.Name;
            var model = await OfferAccessor.GetUserOffersAsync(Context, username);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ReservedOffers()
        {
            string username = HttpContext.User?.Identity?.Name;
            var model = await OfferAccessor.GetUserReservedOffersAsync(Context, username);
            return View(model);
        }

        [HttpGet]
        public ActionResult AddOffer()
        {
            return View();
        }

        [HttpPost]
        [AuthorizationRequired]
        public async Task<ActionResult> AddOffer(AddNewOfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                OfferValidator offerValidator = new OfferValidator(model);
                var errorList = offerValidator.ValidateOffer();
                if (errorList.Count != 0)
                {
                    int i = 0;
                    foreach (var error in errorList)
                    {
                        ModelState.AddModelError(String.Format("{0}", i++), error);
                        return View(model);
                    }
                }
                else
                {
                    string username = HttpContext.User?.Identity?.Name;
                    if (await OfferAccessor.SaveOfferAsync(Context, model, username))
                    {
                        return RedirectToAction("MyOffers", "Offer");
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }

                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditOffer(int id)
        {
            AddNewOfferViewModel offer = await OfferAccessor.GetOfferByIdAsync(Context, id);
            if (offer == null)
            {
                return RedirectToAction("Error");
            }
            return View(offer);
        }

        [HttpPost]
        [AuthorizationRequired]
        public async Task<ActionResult> EditOffer(AddNewOfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                OfferValidator offerValidator = new OfferValidator(model);
                var errorList = offerValidator.ValidateOffer();
                if (errorList.Count != 0)
                {
                    int i = 0;
                    foreach (var error in errorList)
                    {
                        ModelState.AddModelError(String.Format("{0}", i++), error);
                        return View(model);
                    }
                }
                else
                {
                    string username = HttpContext.User?.Identity?.Name;
                    if (await OfferAccessor.EditOfferAsync(Context, model))
                    {
                        return RedirectToAction("MyOffers", "Offer");
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }

                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteOffer(int id)
        {
            string username = HttpContext.User?.Identity?.Name;
            if (await OfferAccessor.DeleteOfferByIdAsync(Context, id, username))
            {
                return RedirectToAction("MyOffers", "Offer");
            }
            else
            {
                return View("Error");
            }

        }
    }
}