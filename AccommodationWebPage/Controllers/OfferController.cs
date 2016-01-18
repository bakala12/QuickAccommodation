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
    /// <summary>
    /// Kontroler odpowiadający za operacje związane z ofertami
    /// </summary>
    public class OfferController : AccommodationController
    {
        /// <summary>
        /// Inicjalizuje nową instancję kontrolera
        /// </summary>
        /// <param name="provider">Dostawca kontekstu bazy danych</param>
        public OfferController(IContextProvider provider) : base(provider) { }

        /// <summary>
        /// Inicajlizuje nową instancję kontrolera.
        /// </summary>
        public OfferController() : base(new ContextProvider<AccommodationContext>()) { }

        /// <summary>
        /// Zwraca listę ofert dodanych przez użytkownika
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MyOffers()
        {
            string username = HttpContext.User?.Identity?.Name;
            var model = await OfferAccessor.GetUserOffersAsync(Context, username);
            return View(model);
        }

        /// <summary>
        /// Zwraca list ofert zarezerwowanych przez użytkownika
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ReservedOffers()
        {
            string username = HttpContext.User?.Identity?.Name;
            var model = await OfferAccessor.GetUserReservedOffersAsync(Context, username);
            return View(model);
        }

        /// <summary>
        /// Zwraca widok dla dodawania nowej oferty
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOffer()
        {
            return View();
        }

        /// <summary>
        /// Dodaje nową ofertę do bazy
        /// </summary>
        /// <param name="model">model z danymi oferty</param>
        /// <param name="image">opcjonalny obrazek</param>
        /// <returns>W przypadku powodzenia zwraca widok ofert użytkownika, w przeciwnym
        /// przypadku widok z informacją o błędzie</returns>
        [HttpPost]
        [AuthorizationRequired]
        public async Task<ActionResult> AddOffer(AddNewOfferViewModel model, HttpPostedFileBase image = null)
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
                    if (await OfferAccessor.SaveOfferAsync(Context, model, username, image))
                    {
                        return RedirectToAction("MyOffers", "Offer");
                    }
                    else
                    {
                        return View("Error", "Offer");
                    }

                }
            }
            return View(model);
        }

        /// <summary>
        /// Umożliwia edycję oferty o danym id
        /// </summary>
        /// <param name="id">id oferty do edycji</param>
        /// <returns>Zwraca widok dla edycji oferty. W przypadku błędu widok 
        /// z informacją o błędzie</returns>
        [HttpGet]
        public async Task<ActionResult> EditOffer(int id)
        {
            AddNewOfferViewModel offer = await OfferAccessor.GetOfferByIdAsync(Context, id);
            if (offer == null)
            {
                return View("Error", "Offer");
            }
            return View(offer);
        }

        /// <summary>
        /// Edytuje w bazie ofertę o danym id
        /// </summary>
        /// <param name="model">Dane oferty do edycji (w tym id)</param>
        /// <returns>Zwraca widok ofert użytkownika. W przypadku błędu, widok 
        /// z informacją o błędzie</returns>
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
                        return View("Error", "Offer");
                    }

                }
            }
            return View(model);
        }

        /// <summary>
        /// Usuwa ofertę o danym id z bazy 
        /// </summary>
        /// <param name="id">id oferty do usunięcia</param>
        /// <returns>Zwraca widok ofert użytkownika. W przypadku błędu, widok 
        /// z informacją o błędzie</returns>
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
                return View("Error", "Offer");
            }

        }
    }
}