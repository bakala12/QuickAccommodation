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
    [AuthorizationRequired]
    public class StatisticsController : AccommodationController
    {
        public StatisticsController(IContextProvider provider) : base(provider) { }

        public StatisticsController():base(new ContextProvider<AccommodationContext>()) { }

        private readonly StatisticsDataAccess _statisticsDataAccess = new StatisticsDataAccess();

        public async Task<ActionResult> Index()
        {
            StatisticsViewModel model =
                await _statisticsDataAccess.GetUserStatisticsAsync(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

        public ActionResult MyAveragePrices()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

        public ActionResult MyOffersCount()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            string[] months;
            model.ThisYearOffersCountOnMonth=RemoveZeroes(model.ThisYearOffersCountOnMonth, out months);
            model.Months = months;
            return View(model);
        }

        public ActionResult MyReservedOffersCount()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            string[] months;
            model.ThisYearReservedOffersCountOnMonth = RemoveZeroes(model.ThisYearReservedOffersCountOnMonth, out months);
            model.Months = months;
            return View(model);
        }

        public ActionResult MyReservedOffersPrices()
        {
            StatisticsViewModel model =
                 _statisticsDataAccess.GetUserStatistics(Context, HttpContext.User?.Identity?.Name);
            return View(model);
        }

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