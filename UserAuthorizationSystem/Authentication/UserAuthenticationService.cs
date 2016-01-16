using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;

namespace UserAuthorizationSystem.Authentication
{
    /// <summary>
    /// Zapewnia możliwość autoryzacji użytkownika implementując IUserAuthorizatonService.
    /// </summary>
    public class UserAuthenticationService: IUserAuthenticationService
    {
        /// <summary>
        /// Autoryzuje użytkownika o podanym loginie i haśle.
        /// </summary>
        /// <typeparam name="T">Parametr generyczny z bazą danych. Musi dać się utworzyć konstruktorem bezparametrowym 
        /// i zawierać informacje o użytkownikach implementując interfejs IUserContext.
        /// </typeparam>
        /// <param name="username">Nazwa użytkownika do autoryzacji.</param>
        /// <param name="password">Hasło użytkownika do autoryzacji (plain text).</param>
        /// <returns>Obiekt CustomIdentity odpowiadający użytkownikowi lub null gdy autoryzacja przebiegła niepomyślnie.</returns>
        public CustomIdentity AuthenticateUser<T>(string username, string password) where T : IUsersContext, IDisposable, new()
        {
            using (var context = new T())
            {
                User user = context.Users.FirstOrDefault(x => x.Username.Equals(username));
                if (user == null || user.Username.Length!=username.Length) return null;
                string hash = PasswordHashHelper.CalculateHash(password, user.Salt);
                return user.HashedPassword.Equals(hash) ? new CustomIdentity(username) : null;
            }
        }

        /// <summary>
        /// Asynchroniczna wersja autoryzacji uzytkownika.
        /// </summary>
        /// <typeparam name="T">Parametr generyczny z bazą danych. Musi dać się utworzyć konstruktorem bezparametrowym 
        /// i zawierać informacje o użytkownikach implementując interfejs IUserContext.
        /// </typeparam>
        /// <param name="username">Nazwa użytkownika do autoryzacji.</param>
        /// <param name="password">Hasło użytkownika do autoryzacji (plain text).</param>
        /// <returns>Obiekt CustomIdentity odpowiadający użytkownikowi lub null gdy autoryzacja przebiegła niepomyślnie.</returns>
        public async Task<CustomIdentity> AuthenticateUserAsync<T>(string username, string password)
            where T : IUsersContext, IDisposable, new()
        {
            return await Task.Run(() => AuthenticateUser<T>(username, password));
        }
    }
}
