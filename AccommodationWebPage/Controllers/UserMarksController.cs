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
    public class UserMarksController : AccommodationController
    {
        /// <summary>
        /// Inicjalizuje nową instancję kontrolera
        /// </summary>
        /// <param name="provider">Dostawca kontekstu bazy danych</param>
        public UserMarksController(IContextProvider provider) : base(provider) { }

        /// <summary>
        /// Inicajlizuje nową instancję kontrolera.
        /// </summary>
        public UserMarksController() : base(new ContextProvider<AccommodationContext>()) { }

        /// <summary>
        /// Zwraca listę ofert dodanych przez użytkownika
        /// </summary>
        /// <returns></returns>
        [AuthorizationRequired]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string username = HttpContext.User?.Identity?.Name;
            var model = await OfferAccessor.GetUsersToMarkAsync(Context, username);
            return View(model);
        }

        [AuthorizationRequired]
        [HttpGet]
        public ActionResult MarkUser(string user, int id )
        {
            return View(new MarkViewModel(user,id));
        }

        [AuthorizationRequired]
        [HttpPost]
        public async Task<ActionResult> MarkUser(MarkViewModel model)
        {
      
         
            if(OfferAccessor.GiveUserMark(Context,model))
            {
                return RedirectToAction("Done", "UserMarks");
            }
            else
            {
                return RedirectToAction("Error", "UserMarks");
            }
        }

        public ActionResult Done()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}