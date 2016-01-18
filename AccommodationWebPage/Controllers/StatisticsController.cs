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
    }
}