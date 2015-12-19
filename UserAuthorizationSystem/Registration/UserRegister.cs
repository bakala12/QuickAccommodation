using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace UserAuthorizationSystem.Registration
{
    /// <summary>
    /// Zapewnia wsparcie dla rejestracji użytkownika w bazie danych.
    /// </summary>
    public class UserRegister : IRegisterUser
    {
        /// <summary>
        /// Tworzy nowego użytkownika w oparciu o podane dane.
        /// </summary>
        /// <param name="username">Nazwa uzytkownika</param>
        /// <param name="clearTextPassword">Hasł użytkownika</param>
        /// <returns>Nowa instancja użytkownika.</returns>
        public User GetNewUser(string username, string clearTextPassword)
        {
            User user = new User();
            user.Username = username;
            string salt = PasswordHashHelper.CreateSalt();
            user.Salt = salt;
            user.HashedPassword = PasswordHashHelper.CalculateHash(clearTextPassword, salt);
            return user;
        }
        /// <summary>
        /// Zapisuje użytkownika z jego danymi do bazy danych.
        /// </summary>
        /// <typeparam name="T">Typ contekstu bazy danych z użytkownikami.</typeparam>
        /// <param name="user">Nazwa użytkownika</param>
        /// <param name="userdata">Dane osobowe użytkownika</param>
        /// <param name="address">Dane adresowe użytkownika</param>
        public void SaveUser<T>(User user, UserData userdata, Address address) where T : IAccommodationContext, IDisposable, new()
        {
            using (var context = new T())
            {
                using (var scope = new TransactionScope())
                {
                    user.UserData = userdata;
                    user.UserData.Address = address;
                    user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Nowicjusz"));
                    context.Users.Add(user);
                    context.SaveChanges();
                    scope.Complete();
                }
            }
        }
        /// <summary>
        /// Asynchronicznie zapisuje użytkownika z jego danymi do bazy danych.
        /// </summary>
        /// <typeparam name="T">Typ contekstu bazy danych z użytkownikami.</typeparam>
        /// <param name="user">Nazwa użytkownika</param>
        /// <param name="userdata">Dane osobowe użytkownika</param>
        /// <param name="address">Dane adresowe użytkownika</param>
        public async Task SaveUserAsync<T>(User user, UserData userdata, Address address) where T : IAccommodationContext, IDisposable, new()
        {
            await Task.Run(() => SaveUser<T>(user, userdata, address));
        }
    }
}
