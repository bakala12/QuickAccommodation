using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTestProject2;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;

namespace UnitTestProject
{
    [TestClass]
    public class LoginTests
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void AuthenticateBakalamUser()
        {
            string username = "bakalam";
            string password = "bakalam00";
            var auth = new UserAuthenticationService();
            var identity = auth.AuthenticateUser<MockContext>(username, password);
            Assert.IsFalse(identity is AnonymousIdentity);
            Assert.IsNotNull(identity);
            Assert.AreEqual(identity.Username, "bakalam");
        }
    }
}
