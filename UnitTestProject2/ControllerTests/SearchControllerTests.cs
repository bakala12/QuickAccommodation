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

        [TestMethod]
        public void SearchByPlaceTest()
        {
            PlaceSearchRequestDto dto = new PlaceSearchRequestDto()
            {
                PlaceName = "Pensjonat pod Różą",
                CityName = "Warszawa",
                SortBy = SortBy.City,
                SortType = SortType.NotSort,
                Username = "bakalam"
            };

            IHttpActionResult result = _controller.SearchByPlace(dto);
            var contentResult = result as OkNegotiatedContentResult<List<Offer>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2,contentResult.Content.Count);
        }

        [TestMethod]
        public void SearchByPlaceTest2WithSorting()
        {
            PlaceSearchRequestDto dto = new PlaceSearchRequestDto()
            {
                CityName = "Warszawa",
                SortBy = SortBy.Price,
                SortType = SortType.Ascending,
                Username = "bakalam"
            };

            IHttpActionResult result = _controller.SearchByPlace(dto);
            var contentResult = result as OkNegotiatedContentResult<List<Offer>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Count);
            Assert.AreEqual(2, contentResult.Content[0].Id);
            Assert.AreEqual(1, contentResult.Content[2].Id);
        }
    }
}
