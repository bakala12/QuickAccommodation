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
using AccommodationWebPage.Validation;
using AccomodationWebApi.Providers;

namespace AccommodationWebPage.Controllers
{
    /// <summary>
    /// Controller for searching offers
    /// </summary>
    [AuthorizationRequired]
    public class SearchController : AccommodationController
    {
        /// <summary>
        /// Initializes a new instance of controller with a specified IContextProvider
        /// </summary>
        /// <param name="provider">Db context provider</param>
        public SearchController(IContextProvider provider) : base(provider) { }

        /// <summary>
        /// Initializes a new instance of controller with a default provider.
        /// </summary>
        public SearchController() : base(new ContextProvider<AccommodationContext>()) { }

        private readonly SearchDataAccessor _searchDataAccessor = new SearchDataAccessor();

        /// <summary>
        /// Gets the general searching view
        /// </summary>
        /// <returns>Searching view</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Place", "Search");
        }

        /// <summary>
        /// Gets the searching by place view
        /// </summary>
        /// <returns>Searching by place view</returns>
        [HttpGet]
        public ActionResult Place()
        {
            PlaceSearchingModel model = new PlaceSearchingModel();
            return View(model);
        }

        /// <summary>
        /// Searches offers by place
        /// </summary>
        /// <param name="model">Model with searching criteria</param>
        /// <returns>Searching by place view with results</returns>
        [HttpPost]
        public async Task<ActionResult> Place(PlaceSearchingModel model)
        {
            model.Username = HttpContext.User?.Identity?.Name;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Niepoprawne dane");
                return View(model);
            }
            IList<OfferViewModel> models = await _searchDataAccessor.SearchByPlaceAsync(Context, model);
            if (models == null)
            {
                ModelState.AddModelError("","Błąd wyszukiwania");
            }
            model.Offers = models ?? new List<OfferViewModel>();
            return View(model);
        }

        /// <summary>
        /// Gets the view for searching by date 
        /// </summary>
        /// <returns>Searching by date view</returns>
        [HttpGet]
        public ActionResult Date()
        {
            DateSearchingModel model = new DateSearchingModel();
            return View(model);
        }

        /// <summary>
        /// Searches offers by date
        /// </summary>
        /// <param name="model">Model with searching criteria</param>
        /// <returns>Searching by date view with results</returns>
        [HttpPost]
        public async Task<ActionResult> Date(DateSearchingModel model)
        {
            model.Username = HttpContext.User?.Identity?.Name;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Niepoprawne dane");
                return View(model);
            }
            IList<OfferViewModel> models = await _searchDataAccessor.SearchByDateAsync(Context, model);
            if (models == null)
            {
                ModelState.AddModelError("", "Błąd wyszukiwania");
            }
            model.Offers = models ?? new List<OfferViewModel>();
            return View(model);
        }

        /// <summary>
        /// Gets the searching by price view
        /// </summary>
        /// <returns>Searching by price view</returns>
        [HttpGet]
        public ActionResult Price()
        {
            PriceSearchingModel model = new PriceSearchingModel();
            return View(model);
        }

        /// <summary>
        /// Searches offers by price
        /// </summary>
        /// <param name="model">Model with searching criterion</param>
        /// <returns>Searching by price view with results</returns>
        [HttpPost]
        public async Task<ActionResult> Price(PriceSearchingModel model)
        {
            model.Username = HttpContext.User?.Identity?.Name;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Niepoprawne dane");
                return View(model);
            }
            if (!string.IsNullOrEmpty(ValidatePrices(model.MinimalPrice, model.MaximalPrice)))
            {
                ModelState.AddModelError("", ValidatePrices(model.MinimalPrice, model.MaximalPrice));
                return View(model);
            }
            IList<OfferViewModel> models = await _searchDataAccessor.SearchByPriceAsync(Context, model);
            if (models == null)
            {
                ModelState.AddModelError("", "Błąd wyszukiwania");
            }
            model.Offers = models ?? new List<OfferViewModel>();
            return View(model);
        }

        /// <summary>
        /// Gets advanced searching view
        /// </summary>
        /// <returns>Advanced searching view</returns>
        [HttpGet]
        public ActionResult Advanced()
        {
            AdvancedSearchingModel model = new AdvancedSearchingModel();
            return View(model);
        }

        /// <summary>
        /// Searches offers by multiple criteria
        /// </summary>
        /// <param name="model">Model with searching criteria</param>
        /// <returns>Advanced searching view with results</returns>
        [HttpPost]
        public async Task<ActionResult> Advanced(AdvancedSearchingModel model)
        {
            model.Username = HttpContext.User?.Identity?.Name;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Niepoprawne dane");
                return View(model);
            }
            if (!string.IsNullOrEmpty(ValidatePrices(model.MinimalPrice, model.MaximalPrice)))
            {
                ModelState.AddModelError("", ValidatePrices(model.MinimalPrice, model.MaximalPrice));
                return View(model);
            }
            IList<OfferViewModel> models = await _searchDataAccessor.SearchByMultipleCriteriaAsync(Context, model);
            if (models == null)
            {
                ModelState.AddModelError("", "Błąd wyszukiwania");
            }
            model.Offers = models ?? new List<OfferViewModel>();
            return View(model);
        }

        /// <summary>
        /// Validates the prices
        /// </summary>
        /// <param name="minPrice">Minimal price</param>
        /// <param name="maxPrice">Maximal price</param>
        /// <returns>String with eventual error message</returns>
        private string ValidatePrices(string minPrice, string maxPrice)
        {
            if (!string.IsNullOrEmpty(minPrice))
            {
                double min;
                if (double.TryParse(minPrice, out min))
                {
                    if (min < 0 || !char.IsDigit(minPrice[0]))
                        return "Niepoprawna cena minimalna";
                }
                else
                {
                    return "Niepoprawna cena minimalna";
                }
            }
            if (!string.IsNullOrEmpty(maxPrice))
            {
                double max;
                if (double.TryParse(maxPrice, out max))
                {
                    if (max < 0 || !char.IsDigit(maxPrice[0]))
                        return "Niepoprawna cena maksymalna";
                }
                else
                {
                    return "Niepoprawna cena maksymalna";
                }
            }
            return string.Empty;
        }
    }
}