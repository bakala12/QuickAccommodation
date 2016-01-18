using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccommodationWebPage.Controllers;
using AccommodationWebPage.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject2.MvcControllersTests
{
    /// <summary>
    /// Test for Account controller
    /// </summary>
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _controller;
        private AccommodationMockContext _context;

        /// <summary>
        /// initialization
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _context = new AccommodationMockContext();
            _controller = new AccountController(new TestContextProvider(_context));    
        }

        /// <summary>
        /// Tests whether registrationprocess ended up with success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task RegisterTestPassed()
        {
            int users = _context.Users.Count();
            RegisterViewModel model = new RegisterViewModel()
            {
                Email = "bakala12@o2.pl",
                FirstName = "Mateusz",
                LastName = "Bąkała",
                Password = "mateusz00",
                PasswordConfirmed = "mateusz00",
                Username = "mateusz"
            };
            ActionResult result = await _controller.Register(model);
            RedirectToRouteResult res = result as RedirectToRouteResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(users+1,_context.Users.Count());
        }

        /// <summary>
        /// Tests whether registration process failed
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task RegisterTestFailed()
        {
            int users = _context.Users.Count();
            RegisterViewModel model = new RegisterViewModel()
            {
                Email = "",
                FirstName = "Mateusz",
                LastName = "Bąkała",
                Password = "mateusz00",
                PasswordConfirmed = "mateusz00",
                Username = "mateusz"
            };
            ActionResult result = await _controller.Register(model);
            Assert.IsNotNull(result);
            Assert.AreEqual(users+1, _context.Users.Count());
        }

        /// <summary>
        /// Tests whether logging in trial failed
        /// </summary>
        [TestMethod]
        public void LoginTestFailed()
        {
            LoginViewModel model = new LoginViewModel()
            {
                Username = "login",
                Password = "password"
            };
            ActionResult res = _controller.Login(model, null);
            Assert.IsNotNull(res);
            Assert.IsFalse(res is RedirectToRouteResult);
        }

        /// <summary>
        /// Tests whether logging in trial successed
        /// </summary>
        [TestMethod]
        public void LoginTestPassed()
        {
            LoginViewModel model = new LoginViewModel()
            {
                Username = "bakalam",
                Password = "bakalam00"
            };
            ActionResult res = _controller.Login(model, "lalalal");
            Assert.IsNotNull(res);
            Assert.IsTrue(res is RedirectToRouteResult);
        }

        /// <summary>
        /// Tests whether logging off trial successed
        /// </summary>
        [TestMethod]
        public void LogoffTestPassed()
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("bakalam");
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            _controller.ControllerContext = mock.Object;
            var result = _controller.LogOff();
            Assert.IsNotNull(result);
            Assert.IsTrue(result is RedirectToRouteResult);
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual("Home", ((RedirectToRouteResult)result).RouteValues["controller"]);
        }
    }
}
