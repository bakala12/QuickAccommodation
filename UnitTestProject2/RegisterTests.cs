using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserAuthorizationSystem.Registration;

namespace UnitTestProject2
{
    /// <summary>
    /// Klasa testująca system rejestracji użytkownika
    /// </summary>
    [TestClass]
    public class RegisterTests
    {
        /// <summary>
        /// Testuje początkową liczbę użytkowników
        /// </summary>
        [TestMethod]
        public void UsersCountInitTest()
        {
            MockContext context=new MockContext();
            int num = context.Users.Count();
            Assert.AreEqual(3, num);
        }

        /// <summary>
        /// Test dodawania użytkownika
        /// </summary>
        [TestMethod]
        public void AddNewUserTest()
        {
            User u = new User() {Username = "chinol"};
            UserData data = new UserData()
            {
                FirstName = "Jan",
                LastName = "Ban",
                Email = "jan@ban.pl"
            };
            Address a = new Address()
            {
                Street = "Jana Bana",
                LocalNumber = "2",
                PostalCode = "11-111",
                City = "Dzbanów"
            };
            IRegisterUser reg=new UserRegister();
            reg.SaveUser<MockContext>(u,data, a);
            MockContext mock=new MockContext();
        }

        /// <summary>
        /// Testuje asynchroniczne dodawanie nowego użytkownika
        /// </summary>
        /// <returns></returns>
        public async Task AddNewUserAsyncTest()
        {
            User u = new User() { Username = "chinol" };
            UserData data = new UserData()
            {
                FirstName = "Jan",
                LastName = "Ban",
                Email = "jan@ban.pl"
            };
            Address a = new Address()
            {
                Street = "Jana Bana",
                LocalNumber = "2",
                PostalCode = "11-111",
                City = "Dzbanów"
            };
            IRegisterUser reg = new UserRegister();
            await reg.SaveUserAsync<MockContext>(u, data, a);
            MockContext mock = new MockContext();
        }
    }
}
