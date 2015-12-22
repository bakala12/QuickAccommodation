using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Controllers;
using AccomodationWebApi.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2.ControllerTests
{
    [TestClass]
    public class OtherControllersTests
    {
        private readonly AccommodationMockContext _context = new AccommodationMockContext();
        private readonly StatisticsController _controller;

        public OtherControllersTests()
        {
            IContextProvider provider = new TestContextProvider(_context);
            _controller = new StatisticsController(provider);
        }

        [TestMethod]
        public void StatisticsCheapestOfferTest()
        {
            string username = "bakalam";

            IHttpActionResult result = _controller.CheapestOfferPrice(username);
            var contentResult = result as OkNegotiatedContentResult<double>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(300, contentResult.Content);
        }

        [TestMethod]
        public void StatisticsMostExpensiveOfferTest()
        {
            string username = "bakalam";

            IHttpActionResult result = _controller.MostExpensiveOfferPrice(username);
            var contentResult = result as OkNegotiatedContentResult<double>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(400, contentResult.Content);
        }
    }
}
