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
    /// <summary>
    /// Testy dla systemu logowania
    /// </summary>
    [TestClass]
    public class LoginTests
    {
        /// <summary>
        /// Sprawdza czy powiedzie się uwierzytelnianie użytkownika bakalam loginem i hasłem
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

        /// <summary>
        /// Prawdza czy powiedzie się autoryzacja użytkownika admin.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AuthenticateAdminUserAsync()
        {
            string username = "admin";
            string password = "admin123";
            var auth = new UserAuthenticationService();
            var identity = await auth.AuthenticateUserAsync<MockContext>(username, password);
            Assert.IsFalse(identity is AnonymousIdentity);
            Assert.IsNotNull(identity);
            Assert.AreEqual(identity.Username, "admin");
        }

        /// <summary>
        /// Test czy autentykacja z nieprawidłowym loginem się powiedzie (nie powinna)
        /// </summary>
        [TestMethod]
        public void AuthenticationFailedWithNonExistingUser()
        {
            string username = "admin1";
            string password = "admin123";
            var auth = new UserAuthenticationService();
            var identity = auth.AuthenticateUser<MockContext>(username, password);
            Assert.IsNull(identity);
        }

        /// <summary>
        /// Test czy autentykacja z nieprawidłowym hasłem się powiedzie
        /// </summary>
        [TestMethod]
        public void AuthenticationPasswordFailed()
        {
            string username = "admin";
            string password = "admin12";
            var auth = new UserAuthenticationService();
            var identity = auth.AuthenticateUser<MockContext>(username, password);
            Assert.IsNull(identity);
        }

        /// <summary>
        /// Test z logowaniem pustych loginu i hasła
        /// </summary>
        [TestMethod]
        public void AuthenticationWithEmptyLogin()
        {
            string username=string.Empty;
            string password = string.Empty;
            var auth=new UserAuthenticationService();
            var identity = auth.AuthenticateUser<MockContext>(username, password);
            Assert.IsNull(identity);
        }

    }
}
