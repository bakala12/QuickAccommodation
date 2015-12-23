using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccommodationShared.Searching;
using AccomodationWebApi.Controllers;
using AccomodationWebApi.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2.ControllerTests
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
        }

        /// <summary>
        /// Tests searching by place.
        /// </summary>
        [TestMethod]
        public void SearchByPlaceTest()
        {
            PlaceSearchRequestDto dto = new PlaceSearchRequestDto()
            {
                PlaceName = "Pensjonat pod różą",
                CityName = "Warszawa",
                SortBy = SortBy.City,
                SortType = SortType.NotSort,
                Username = "bakalam"
            };

            IHttpActionResult result = _controller.SearchByPlace(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1,contentResult.Content.Offers.Count());
        }

        /// <summary>
        /// Tests searching by place.
        /// </summary>
        [TestMethod]
        public void SearchByPlaceTest2WithSorting()
        {
            PlaceSearchRequestDto dto = new PlaceSearchRequestDto()
            {
                CityName = "Warszawa",
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            IHttpActionResult result = _controller.SearchByPlace(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Offers.Count());
            Assert.AreEqual(2, contentResult.Content.Offers.ToList()[0].Id);
            Assert.AreEqual(4, contentResult.Content.Offers.ToList()[1].Id);
        }

        /// <summary>
        /// Tests searching by price.
        /// </summary>
        [TestMethod]
        public void SearchByPriceTest1()
        {
            PriceSearchRequestDto dto = new PriceSearchRequestDto()
            {
                MinimalPrice = 300,
                MaximalPrice = 350,
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            IHttpActionResult result = _controller.SearchByPrice(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Offers.Count());
        }

        /// <summary>
        /// Tests searching by price.
        /// </summary>
        [TestMethod]
        public void SearchByPriceTest2()
        {
            PriceSearchRequestDto dto = new PriceSearchRequestDto()
            {
                MinimalPrice = 305,
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            IHttpActionResult result = _controller.SearchByPrice(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Offers.Count());
            Assert.AreEqual(400, contentResult.Content.Offers.First().OfferInfo.Price);
        }

        /// <summary>
        /// Tests searching by date.
        /// </summary>
        [TestMethod]
        public void SearchByDateTest1()
        {
            DateSearchRequestDto dto = new DateSearchRequestDto()
            {
                MinimalDate = new DateTime(2015,12,20),
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim",
                ShowPartiallyMatchingResults = true
            };

            IHttpActionResult result = _controller.SearchByDate(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Offers.Count());
        }

        /// <summary>
        /// Tests searching by date.
        /// </summary>
        [TestMethod]
        public void SearchByDateTest2()
        {
            DateSearchRequestDto dto = new DateSearchRequestDto()
            {
                MinimalDate = new DateTime(2015, 12, 20),
                MaximalDate = new DateTime(2015,12,22),
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "jablonskim"
            };

            IHttpActionResult result = _controller.SearchByDate(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(0, contentResult.Content.Offers.Count());
        }

        /// <summary>
        /// Tests searching by multiple criteria.
        /// </summary>
        [TestMethod]
        public void AdvancedSearchingTest1()
        {
            AdvancedSearchRequestDto dto = new AdvancedSearchRequestDto()
            {
                Username = "jablonskim",
                MinimalDate = new DateTime(2015, 12, 20),
                MinimalPrice = 300,
                PlaceName = "Pensjonat pod różą"
            };
        }
        /// <summary>
         /// Tests searching by multiple criteria.
         /// </summary>
        [TestMethod]
        public void AdvancedSearchingTest2()
        {
            AdvancedSearchRequestDto dto = new AdvancedSearchRequestDto()
            {
                Username = "jablonskim",
                MinimalDate = new DateTime(2015, 12, 20),
                MinimalPrice = 300,
                PlaceName = "Pensjonat pod różą",
                MaximalPrice = 301
            };

            IHttpActionResult result = _controller.SearchByMultipleCriteria(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Offers.Count());
        }

        /// <summary>
        /// Tests advanced searching functionality.
        /// </summary>
        [TestMethod]
        public void AdvancedSearchingTest3()
        {
            AdvancedSearchRequestDto dto = new AdvancedSearchRequestDto()
            {
                Username = "jablonskim",
                MinimalDate = new DateTime(2015, 12, 20),
                MaximalPrice = 300,
                PlaceName = "Pensjonat pod różą",
            };

            IHttpActionResult result = _controller.SearchByMultipleCriteria(dto);
            var contentResult = result as OkNegotiatedContentResult<SearchResultDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Offers.Count());
            Assert.AreEqual(300, contentResult.Content.Offers.First().OfferInfo.Price);
            Assert.AreEqual(2, contentResult.Content.Offers.First().Id);
        }
    }
}
