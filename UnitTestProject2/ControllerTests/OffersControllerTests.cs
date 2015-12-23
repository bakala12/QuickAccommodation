using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using AccommodationDataAccess.Model;
using AccomodationWebApi.Controllers;
using AccomodationWebApi.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccommodationShared.Dtos;
using System.Data.Entity;

namespace UnitTestProject2.ControllerTests
{
    /// <summary>
    /// Tests for OffersController class.
    /// </summary>
    [TestClass]
    public class OffersControllerTests
    {
        private readonly AccommodationMockContext _context = new AccommodationMockContext();
        private readonly OffersController _controller;

        /// <summary>
        /// Initializes controller and mocked database.
        /// </summary>
        public OffersControllerTests()
        {
            IContextProvider provider = new TestContextProvider(_context);
            _controller = new OffersController(provider);
        }

        /// <summary>
        /// Tests getting offer from database by Id.
        /// </summary>
        [TestMethod]
        public void GetOfferByIdTest()
        {
            int id = 1;
            IHttpActionResult result = _controller.Get(id);
            var contentResult = result as OkNegotiatedContentResult<Offer>;
            Offer o = _context.Offers.FirstOrDefault(of => of.Id == id);

            Assert.IsNotNull(o);
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(o.VendorId, contentResult.Content.VendorId);
        }

        /// <summary>
        /// Tests getting all offers for the given user as vendor.
        /// </summary>
        [TestMethod]
        public void GetUserOffersTest()
        {
            IHttpActionResult result = _controller.GetUserOffers(1);
            var contentResult = result as OkNegotiatedContentResult<IList<Offer>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Count);
        }

        /// <summary>
        /// Tests reserving offer functionality.
        /// </summary>
        [TestMethod]
        public void ReserveOfferTest()
        {
            ReserveOfferDto dto = new ReserveOfferDto()
            {
                Username = "jablonskim",
                OfferId = 2
            };

            IHttpActionResult result = _controller.ReserveOffer(dto);
            var contentResult = result as OkNegotiatedContentResult<bool>;

            Offer o = _context.Offers.FirstOrDefault(x => x.Id == dto.OfferId);

            Assert.IsNotNull(o);
            Assert.AreEqual(true,o.IsBooked);
            Assert.IsTrue(o.Customer.Username.Equals("jablonskim"));
        }

        /// <summary>
        /// Tests resigning offer functionality.
        /// </summary>
        [TestMethod]
        public void ResignOfferTest()
        {
            ReserveOfferDto dto = new ReserveOfferDto()
            {
                Username = "jablonskim",
                OfferId = 3
            };

            IHttpActionResult result = _controller.ResignOffer(dto);
            var contentResult = result as OkNegotiatedContentResult<bool>;

            Offer o = _context.Offers.FirstOrDefault(x => x.Id == dto.OfferId);

            Assert.IsNotNull(o);
            Assert.AreEqual(o.IsBooked, false);
            Assert.IsNull(o.Customer);
        }

        /// <summary>
        /// Tests getting all reserved for the current user.
        /// </summary>
        [TestMethod]
        public void GetReservedOffersTest()
        {
            string username = "bakalam";

            IHttpActionResult result = _controller.GetReservedOffers(username);
            var contentResult = result as OkNegotiatedContentResult<IList<Offer>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(0, contentResult.Content.Count);
        }

        /// <summary>
        /// Tests for removing offer functionality.
        /// </summary>
        [TestMethod]
        public void RemoveOfferTest()
        {
            OfferEditDataDto dto = new OfferEditDataDto()
            {
                Username = "bakalam",
                OfferId = 4
            };

            IHttpActionResult result = _controller.RemoveOfferAsync(dto);

            Offer o = _context.Offers.FirstOrDefault(x => x.Id == dto.OfferId);

            Assert.IsNull(o);
        }
    }
}
