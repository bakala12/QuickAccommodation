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
    /// Odpowiada za logikę rezerwacji ofert
    /// </summary>
    [AuthorizationRequired]
    public class ReservationController : AccommodationController
    {
        /// <summary>
        /// Inicjalizuje nową instancję kontrolera
        /// </summary>
        /// <param name="provider">Dostawca kontekstu bazy danych</param>
        public ReservationController(IContextProvider provider) : base(provider) { }

        /// <summary>
        /// Inicajlizuje nową instancję kontrolera.
        /// </summary>
        public ReservationController() : base(new ContextProvider<AccommodationContext>()) { }

        /// <summary>
        /// Rezerwuje obecnemu użytkownikowi ofertę o danym id
        /// </summary>
        /// <param name="id">id oferty do rezerwacji</param>
        /// <returns>Zwraca informację o powodzeniu lub niepowodzeniu rezerwacji</returns>
        public async Task<ActionResult> ReserveOffer(int id)
        {
            string username = HttpContext.User?.Identity?.Name;
            if (await OfferAccessor.ReserveOfferAsync(Context, id, username))
            {
                return RedirectToAction("Done", "Reservation");
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Odpowiada za rezygnację z rezerwacji oferty o danym id
        /// przez obecnego użytkownika
        /// </summary>
        /// <param name="id">id oferty</param>
        /// <returns></returns>
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