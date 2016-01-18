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
    public class ReservationControllerTests
    {
        private ReservationController _controller;
        private AccommodationMockContext _context;

        [TestInitialize]
        public void Init()
        {
            _context = new AccommodationMockContext();
            _controller = new ReservationController(new TestContextProvider(_context));    
        }

        [TestMethod]
        public async Task OfferReservationTest()
        {
            SetMockContext("bakalam");
            Assert.IsFalse(_context.Offers.FirstOrDefault(o => o.Id == 1).IsBooked);
            var result = await _controller.ReserveOffer(1);
            Assert.IsTrue(_context.Offers.FirstOrDefault(o => o.Id == 1).IsBooked);
        }


        [TestMethod]
        public async Task OfferResigningTest()
        {
            SetMockContext("jablonskim");
            Assert.IsTrue(_context.Offers.FirstOrDefault(o => o.Id == 3).IsBooked);
            var result = await _controller.ResignOffer(3);
            Assert.IsFalse(_context.Offers.FirstOrDefault(o => o.Id == 3).IsBooked);
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
