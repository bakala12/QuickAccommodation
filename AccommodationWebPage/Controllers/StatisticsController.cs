using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccommodationWebPage.Authorization;
using AccommodationWebPage.DataAccess;
using AccommodationWebPage.Models;
using AccomodationWebApi.Providers;

namespace AccommodationWebPage.Controllers
{
    /// <summary>
    /// Controller for getting users statistics
    /// </summary>
    [AuthorizationRequired]
    public class StatisticsController : AccommodationController
    {
        /// <summary>
        /// Initializes a new instance of Statistics controller with a specified IContextProvider
        /// </summary>
        /// <param name="provider"></param>
        public StatisticsController(IContextProvider provider) : base(provider) { }

        /// <summary>
        /// Initializes a new instnce of StatisticsController with default db context provider
        /// </summary>
        public StatisticsController():base(new ContextProvider<AccommodationContext>()) { }

        private readonly StatisticsDataAccess _statisticsDataAccess = new StatisticsDataAccess();

        /// <summary>
        /// Gets the statistics view
        /// </summary>
        /// <returns>The statistics view</returns>
        public async Task<ActionResult> Index()
        {
            StatisticsViewModel model =
                await _statisticsDataAccess.GetUserStatisticsAsync(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

        /// <summary>
        /// Gets the view with my offers' prices chart
        /// </summary>
        /// <returns>My offers' prices chart</returns>
        public ActionResult MyAveragePrices()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

        /// <summary>
        /// Genterates myofers' count chart
        /// </summary>
        /// <returns>My offers' chart</returns>
        public ActionResult MyOffersCount()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            string[] months;
            model.ThisYearOffersCountOnMonth=RemoveZeroes(model.ThisYearOffersCountOnMonth, out months);
            model.Months = months;
            return View(model);
        }

        /// <summary>
        /// Gets the chart for my reserved offers' counts
        /// </summary>
        /// <returns>the chart for my reserved offers' counts</returns>
        public ActionResult MyReservedOffersCount()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            string[] months;
            model.ThisYearReservedOffersCountOnMonth = RemoveZeroes(model.ThisYearReservedOffersCountOnMonth, out months);
            model.Months = months;
            return View(model);
        }

        /// <summary>
        /// Gets the chart for my reserved offers' prices
        /// </summary>
        /// <returns>the chart for my reserved offers' prices</returns>
        public ActionResult MyReservedOffersPrices()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        private int[] RemoveZeroes(int[] val, out string[] months)
        {
            months = new[] { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
            List<int> indexesToRemove = new List<int>();
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] == 0)
                {
                    indexesToRemove.Add(i);
                }
            }
            var v = val.ToList();
            var m = months.ToList();
            foreach (var index in indexesToRemove)
            {
                v.Remove(val[index]);
                m.Remove(months[index]);
            }
            months = m.ToArray();
            val = v.ToArray();
            return val;
        }
    }
}