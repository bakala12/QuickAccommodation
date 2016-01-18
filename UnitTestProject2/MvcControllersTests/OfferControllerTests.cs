using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccommodationWebPage.Controllers;
using AccommodationWebPage.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;

namespace UnitTestProject2.MvcControllersTests
{
    [TestClass]
    public class OfferControllerTests
    {
        private OfferController _controller;
        private AccommodationMockContext _context;

        [TestInitialize]
        public void Init()
        {
            _context = new AccommodationMockContext();
            _controller = new OfferController(new TestContextProvider(_context));    
        }

        [TestMethod]
        public async Task GetUserOffersTest()
        {
            SetMockContext("bakalam");
            var result = await _controller.MyOffers();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetUserReservedOffersTest()
        {
            SetMockContext("bakalam");
            var result = await _controller.ReservedOffers();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddNewOfferTest()
        {
            SetMockContext("bakalam");
            AddNewOfferViewModel model = new AddNewOfferViewModel
            {
                AccommodationName = "nowododananazwa",
                City = "nowemiasto",
                Description = "nowyopis",
                StartDate= new DateTime(2015, 12, 25),
                EndDate = new DateTime(2015, 12, 29),
                LocalNumber = "1",
                PostalCode = "11-111",
                Price = "123",
                RoomNumber = "123",
                Street = "nowaulica",
                AvailiableVacanciesNumber = "123",
            };
            int count = _context.Offers.Count();
            var result = await _controller.AddOffer(model);
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task EditOfferTest()
        {
            SetMockContext("bakalam");
            AddNewOfferViewModel model = new AddNewOfferViewModel
            {
                AccommodationName = "nowododananazwa",
                City = "nowemiasto",
                Description = "nowyopis",
                StartDate = new DateTime(2015, 12, 25),
                EndDate = new DateTime(2015, 12, 29),
                LocalNumber = "1",
                PostalCode = "11-111",
                Price = "410",
                RoomNumber = "123",
                Street = "nowaulica",
                AvailiableVacanciesNumber = "123",
                Id = 1
            };


            var price1 = _context.Offers.Include(o => o.OfferInfo).FirstOrDefault(o => o.Id == model.Id).OfferInfo.Price;
            var result = await _controller.EditOffer(model);
            var price2 = _context.Offers.Include(o => o.OfferInfo).FirstOrDefault(o => o.Id == model.Id).OfferInfo.Price;
            Assert.IsNotNull(result);
            Assert.AreNotEqual(price1,price2);
        }


        [TestMethod]
        public async Task DeleteOfferTest()
        {
            SetMockContext("bakalam");


            var count1 = _context.Offers.Count();
            var result = await _controller.DeleteOffer(2);
            var count2 = _context.Offers.Count();
            Assert.IsNotNull(result);
            Assert.AreEqual(count1,count2 + 1);
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
