using System.Web.Http;
using System.Web.Http.Results;
using AccommodationShared.Dtos;
using AccomodationWebApi.Controllers;
using AccomodationWebApi.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2.WebApiControllerTests
{
    /// <summary>
    /// Other controllers tests
    /// </summary>
    [TestClass]
    public class OtherControllersTests
    {
        private readonly AccommodationMockContext _context = new AccommodationMockContext();
        private readonly StatisticsController _controller;
        private readonly UserDataController _controller2;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        public OtherControllersTests()
        {
            IContextProvider provider = new TestContextProvider(_context);
            _controller = new StatisticsController(provider);
            _controller2 = new UserDataController(provider);
        }

        /// <summary>
        /// Tests statistics cheapest offer method.
        /// </summary>
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

        /// <summary>
        /// Tests statistics most expensive offers method.
        /// </summary>
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

        /// <summary>
        /// Tests getting user data.
        /// </summary>
        [TestMethod]
        public void GetUserDataTest()
        {
            IHttpActionResult result = _controller2.GetUserData("bakalam");
            var contentResult = result as OkNegotiatedContentResult<UserBasicDataDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("Mateusz", contentResult.Content.FirstName);
            Assert.AreEqual("Bąkała", contentResult.Content.LastName);
            Assert.AreEqual("bakala12@o2.pl", contentResult.Content.Email);
        }

        /// <summary>
        /// Tests getting user rank from server.
        /// </summary>
        [TestMethod]
        public void GetUserRankTest()
        {
            IHttpActionResult result = _controller2.GetUserRank("bakalam");
            var contentResult = result as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("Nowicjusz", contentResult.Content);
        }
    }
}
