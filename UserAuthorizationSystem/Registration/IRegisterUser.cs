using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace UserAuthorizationSystem.Registration
{
    /// <summary>
    /// Wspomaga rejestrację nowego użytkownika.
    /// </summary>
    public interface IRegisterUser
    {
        /// <summary>
        /// Tworzy nowego użytkownika w oparciu o podane dane.
        /// </summary>
        /// <param name="username">Nazwa uzytkownika</param>
        /// <param name="clearTextPassword">Hasł użytkownika</param>
        /// <returns>Nowa instancja użytkownika.</returns>
        User GetNewUser(string username, string clearTextPassword);
        
        /// <summary>
        /// Zapisuje użytkownika z jego danymi do bazy danych.
        /// </summary>
        /// <typeparam name="T">Typ contekstu bazy danych z użytkownikami.</typeparam>
        /// <param name="user">Nazwa użytkownika</param>
        /// <param name="userdata">Dane osobowe użytkownika</param>
        /// <param name="address">Dane adresowe użytkownika</param>
        void SaveUser<T>(User user, UserData userdata, Address address) where T:IAccommodationContext, IDisposable, new();
        
        /// <summary>
        /// Asynchronicznie zapisuje użytkownika z jego danymi do bazy danych.
        /// </summary>
        /// <typeparam name="T">Typ contekstu bazy danych z użytkownikami.</typeparam>
        /// <param name="user">Nazwa użytkownika</param>
        /// <param name="userdata">Dane osobowe użytkownika</param>
        /// <param name="address">Dane adresowe użytkownika</param>
        Task SaveUserAsync<T>(User user, UserData userdata, Address address) where T:IAccommodationContext, IDisposable, new();

        /// <summary>
        /// Zapisuje użytkownika z jego danymi do bazy danych.
        /// </summary>
        /// <param name="context">Kontekst bazy danych.</param>
        /// <param name="user">Nazwa użytkownika</param>
        /// <param name="userdata">Dane osobowe użytkownika</param>
        /// <param name="address">Dane adresowe użytkownika</param>
        /// <returns>True jeśli operacja zakończyła się suksecem, w przeciwnym wypadklu false.</returns>
        bool SaveUser(IAccommodationContext context, User user, UserData userdata, Address address);

        /// <summary>
        /// Asynchronicznie zapisuje użytkownika z jego danymi do bazy danych.
        /// </summary>
        /// <param name="context">Kontekst bazy danych.</param>
        /// <param name="user">Nazwa użytkownika</param>
        /// <param name="userdata">Dane osobowe użytkownika</param>
        /// <param name="address">Dane adresowe użytkownika</param>
        /// <returns>True jeśli operacja zakończyła się suksecem, w przeciwnym wypadklu false.</returns>
        Task<bool> SaveUserAsync(IAccommodationContext context, User user, UserData userdata, Address address);
    }
}
