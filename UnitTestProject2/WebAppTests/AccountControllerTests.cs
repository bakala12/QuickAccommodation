using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccommodationWebPage.Controllers;
using AccommodationWebPage.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2.WebAppTests
{
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _controller;
        private AccommodationMockContext _context;

        [TestInitialize]
        public void Init()
        {
            _context = new AccommodationMockContext();
            _controller = new AccountController(new TestContextProvider(_context));    
        }

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
    }
}
