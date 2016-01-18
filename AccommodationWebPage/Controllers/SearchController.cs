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
    [AuthorizationRequired]
    public class SearchController : AccommodationController
    {
        public SearchController(IContextProvider provider) : base(provider) { }

        public SearchController() : base(new ContextProvider<AccommodationContext>()) { }

        private readonly SearchDataAccessor _searchDataAccessor = new SearchDataAccessor();

        public ActionResult Index()
        {
            return RedirectToAction("Place", "Search");
        }

        [HttpGet]
        public ActionResult Place()
        {
            PlaceSearchingModel model = new PlaceSearchingModel();
            return View(model);
        }

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

        [HttpGet]
        public ActionResult Date()
        {
            DateSearchingModel model = new DateSearchingModel();
            return View(model);
        }

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

        [HttpGet]
        public ActionResult Price()
        {
            PriceSearchingModel model = new PriceSearchingModel();
            return View(model);
        }

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

        [HttpGet]
        public ActionResult Advanced()
        {
            AdvancedSearchingModel model = new AdvancedSearchingModel();
            return View(model);
        }

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