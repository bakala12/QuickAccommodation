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
            Assert.AreEqual(2, contentResult.Content.Count);
        }
    }
}
