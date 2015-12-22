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
    [TestClass]
    public class OffersControllerTests
    {
        private readonly AccommodationMockContext _context = new AccommodationMockContext();
        private readonly OffersController _controller;

        public OffersControllerTests()
        {
            IContextProvider provider = new ContextProvider<AccommodationMockContext>();
            _controller = new OffersController(provider);
        }

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

        [TestMethod]
        public void GetUserOffersTest()
        {
            IHttpActionResult result = _controller.GetUserOffers(1);
            var contentResult = result as OkNegotiatedContentResult<IList<Offer>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Count);
        }

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

            Assert.AreEqual(o.IsBooked, true);
            Assert.AreEqual(o.CustomerId, 2);
        }


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

            Assert.AreEqual(o.IsBooked, false);
            Assert.AreEqual(o.CustomerId, null);
        }

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

        [TestMethod]
        public void AddOfferTest()
        {

            User vendor = new User
            {
                Username = "jablonskim2",
            };
            OfferInfo offerInfo = new OfferInfo()
            {
                OfferStartTime = new DateTime(2015, 12, 26),
                OfferEndTime = new DateTime(2015, 12, 30),
                OfferPublishTime = new DateTime(2015, 12, 13),
                Price = 500,
            };
            Place place = new Place()
            {
                PlaceName = "Willa Magnolia",
                Address = new Address()
                {
                    City = "Zakopane",
                    Id = 3,
                    PostalCode = "11-111",
                    LocalNumber = "12a",
                    Street = "Zielona"
                }
            };

            Room room = new Room()
            {
                Capacity = 2,
                Number = "3",
            };

            OfferAllDataDto dto = new OfferAllDataDto()
            {
                Vendor = vendor,
                OfferInfo = offerInfo,
                Place = place,
                Room = room
            };

            IHttpActionResult result = _controller.SaveOfferAsync(dto);

          //  OfferInfo offerInfoTest = _context.OfferInfo.Find(offerInfo);
          //  Place placeTest = _context.Places.Find(place);
          //  Room roomtest = _context.Rooms.Find(room);
          ////  Offer offerTest = _context.Offers.FirstOrDefault(x => x.OfferInfoId == offerInfoTest.Id);
          //  User userTest = _context.Users.FirstOrDefault(x => x.Username == "jablonskim2"); 

          //  Assert.AreEqual(roomtest.PlaceId, placeTest.Id);
          //  Assert.AreEqual(offerInfoTest.Price, 500);
          ////  Assert.AreEqual(offerTest.Room, roomtest);
          ////  Assert.AreEqual(offerTest.VendorId, userTest.Id);
        }

    }
}
