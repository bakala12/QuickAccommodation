using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using AccommodationShared.Searching;
using AccommodationWebPage.Controllers;
using AccommodationWebPage.Models;
using AccomodationWebApi.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject2.MvcControllersTests
{
    /// <summary>
    /// Class with test methods for SearchController.
    /// </summary>
    [TestClass]
    public class SearchControllerTests
    {
        private readonly AccommodationMockContext _context;
        private readonly SearchController _controller;

        /// <summary>
        /// Initializes a new instance of SearchControllerTests class.
        /// </summary>
        public SearchControllerTests()
        {
            _context = new AccommodationMockContext();
            _controller = new SearchController(new ContextProvider<AccommodationMockContext>());
            SetMockContext("jablonskim");
        }

        /// <summary>
        /// Tests searching by place.
        /// </summary>
        [TestMethod]
        public async Task SearchByPlaceTest()
        {
            SetMockContext("bakalam");
            PlaceSearchingModel model = new PlaceSearchingModel()
            {
                PlaceName = "Pensjonat pod różą",
                CityName = "Warszawa",
                SortBy = SortBy.City,
                SortType = SortType.NotSort,
                Username = "bakalam"
            };

            ActionResult result = await _controller.Place(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Offers.Count());
        }

        /// <summary>
        /// Tests searching by place.
        /// </summary>
        [TestMethod]
        public async Task SearchByPlaceTest2WithSorting()
        {
            SetMockContext("jablonskim");
            PlaceSearchingModel model = new PlaceSearchingModel()
            {
                CityName = "Warszawa",
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            var result = await _controller.Place(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Offers.Count());
            Assert.AreEqual(2, model.Offers.ToList()[0].Id);
            Assert.AreEqual(4, model.Offers.ToList()[1].Id);
        }

        /// <summary>
        /// Tests searching by price.
        /// </summary>
        [TestMethod]
        public async Task SearchByPriceTest1()
        {
            SetMockContext("jablonskim");
            PriceSearchingModel model = new PriceSearchingModel()
            {
                MinimalPrice = 300,
                MaximalPrice = 350,
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            var result = await _controller.Price(model);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Offers.Count());
        }

        /// <summary>
        /// Tests searching by price.
        /// </summary>
        [TestMethod]
        public async Task SearchByPriceTest2()
        {
            SetMockContext("jablonskim");
            PriceSearchingModel model = new PriceSearchingModel()
            {
                MinimalPrice = 305,
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            var result = await _controller.Price(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Offers.Count());
            Assert.AreEqual(400, model.Offers.First().Price);
        }

        /// <summary>
        /// Tests searching by date.
        /// </summary>
        [TestMethod]
        public async Task SearchByDateTest1()
        {
            SetMockContext("jablonskim");
            DateSearchingModel model = new DateSearchingModel()
            {
                MinimalDate = new DateTime(2015, 12, 20),
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim",
                ShowPartiallyMatchingResults = true
            };

            var result = await _controller.Date(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Offers.Count());
        }

        /// <summary>
        /// Tests searching by date.
        /// </summary>
        [TestMethod]
        public async Task SearchByDateTest2()
        {
            SetMockContext("jablonskim");
            DateSearchingModel model = new DateSearchingModel()
            {
                MinimalDate = new DateTime(2015, 12, 20),
                MaximalDate = new DateTime(2015, 12, 22),
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            var result = await _controller.Date(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, model.Offers.Count());
        }

        /// <summary>
        /// Tests searching by multiple criteria.
        /// </summary>
        [TestMethod]
        public async Task AdvancedSearchingTest1()
        {
            SetMockContext("jablonskim");
            AdvancedSearchingModel model = new AdvancedSearchingModel()
            {
                Username = "jablonskim",
                MinimalDate = new DateTime(2015, 12, 20),
                MinimalPrice = 300,
                PlaceName = "Pensjonat pod różą"
            };

            var result = await _controller.Advanced(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Offers.Count());
        }

        /// <summary>
        /// Tests searching by multiple criteria.
        /// </summary>
        [TestMethod]
        public async Task AdvancedSearchingTest2()
        {
            SetMockContext("jablonskim");
            AdvancedSearchingModel model = new AdvancedSearchingModel()
            {
                Username = "jablonskim",
                MinimalDate = new DateTime(2015, 12, 20),
                MinimalPrice = 300,
                PlaceName = "Pensjonat pod różą",
                MaximalPrice = 301
            };

            var result = await _controller.Advanced(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Offers.Count());
        }

        /// <summary>
        /// Tests advanced searching functionality.
        /// </summary>
        [TestMethod]
        public async Task AdvancedSearchingTest3()
        {
            SetMockContext("jablonskim");
            AdvancedSearchingModel model = new AdvancedSearchingModel()
            {
                Username = "jablonskim",
                MinimalDate = new DateTime(2015, 12, 20),
                MaximalPrice = 300,
                PlaceName = "Pensjonat pod różą",
            };

            var result = await _controller.Advanced(model);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Offers.Count());
            Assert.AreEqual(300, model.Offers.First().Price);
            Assert.AreEqual(2, model.Offers.First().Id);
        }

        private void SetMockContext(string username)
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns(username);
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            _controller.ControllerContext = mock.Object;
        }
    }
}