using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using UserAuthorizationSystem.Identities;

namespace UserAuthorizationSystem.Authentication
{
    /// <summary>
    /// Zapewnia możliwość autoryzacji użytkownika w aplikacji.
    /// </summary>
    public interface IUserAuthenticationService
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
        CustomIdentity AuthenticateUser<T>(string username, string password) where T : IUsersContext, IDisposable, new();

        /// <summary>
        /// Asynchroniczna wersja autoryzacji uzytkownika.
        /// </summary>
        /// <typeparam name="T">Parametr generyczny z bazą danych. Musi dać się utworzyć konstruktorem bezparametrowym 
        /// i zawierać informacje o użytkownikach implementując interfejs IUserContext.
        /// </typeparam>
        /// <param name="username">Nazwa użytkownika do autoryzacji.</param>
        /// <param name="password">Hasło użytkownika do autoryzacji (plain text).</param>
        /// <returns>Obiekt CustomIdentity odpowiadający użytkownikowi lub null gdy autoryzacja przebiegła niepomyślnie.</returns>
        Task<CustomIdentity> AuthenticateUserAsync<T>(string username, string password)
            where T : IUsersContext, IDisposable, new();

        /// <summary>
        /// Autoryzuje użytkownika o podanym loginie i haśle.
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="username">Nazwa użytkownika do autoryzacji.</param>
        /// <param name="password">Hasło użytkownika do autoryzacji (plain text).</param>
        /// <returns>Obiekt CustomIdentity odpowiadający użytkownikowi lub null gdy autoryzacja przebiegła niepomyślnie.</returns>
        CustomIdentity AuthenticateUser(IUsersContext context, string username, string password);

        /// <summary>
        /// Asynchroniczna wersja autoryzacji uzytkownika.
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="username">Nazwa użytkownika do autoryzacji.</param>
        /// <param name="password">Hasło użytkownika do autoryzacji (plain text).</param>
        /// <returns>Obiekt CustomIdentity odpowiadający użytkownikowi lub null gdy autoryzacja przebiegła niepomyślnie.</returns>
        Task<CustomIdentity> AuthenticateUserAsync(IUsersContext context, string username, string password);
    }
}
