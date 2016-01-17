using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationWebPage.Models;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

namespace AccommodationWebPage.Authorization
{
    /// <summary>
    /// Singleton wspomagający rejestrację użytkownika.
    /// </summary>
    internal sealed class Registration
    {
        /// <summary>
        /// Prywatny konstruktor.
        /// </summary>
        private Registration() { }

        private static readonly object SyncRoot = new object();
        private static Registration _instance;

        /// <summary>
        /// Instancja singletnu.
        /// </summary>
        internal static Registration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if(_instance ==null)
                            _instance = new Registration();
                    }
                }
                return _instance;
            }
        } 

        private readonly IUserCredentialsValidator _validator = new UserCredentialsValidator();
        private readonly IRegisterUser _register = new UserRegister();

        /// <summary>
        /// Validuje dane użytkownika.
        /// </summary>
        /// <param name="model">Model z danymi</param>
        /// <param name="errorMessage">Wyjściowy parametr z przyczyną błędu walidacji.</param>
        /// <returns>Informacji o tym czy walidacja przebiegła pomyślnie czy nie.</returns>
        private bool ValidateUserData(RegisterViewModel model, out string errorMessage)
        {
            if (!_validator.ValidateLocalNumber(model.LocalNumber))
            {
                errorMessage = "Numer domu musi zaczynać się cyfrą";
                return false;
            }
            if (!_validator.ValidatePostalCode(model.PostalCode))
            {
                errorMessage = "Nieprawidłowy kod pocztowy";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Waliduje nazwę użytkownika.
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="model">Model z danymi.</param>
        /// <returns>String z ewentualną przyczyną niepowodzenia.</returns>
        public async Task<string> ValidateUserAsync(IAccommodationContext context, RegisterViewModel model)
        {
            string errorMessage;
            bool b = await _validator.ValidateUsernameAsync(context,model.Username);
            if (!b) return "Nazwa użytkownika musi być unikalna. Proszę wybrać unikalną nazwę.";
            return !ValidateUserData(model, out errorMessage) ? errorMessage : string.Empty;
        }

        /// <summary>
        /// Zapisuje użytkownika do bazy danych.
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="model">Model z danymi.</param>
        public async Task SaveUserAsync(IAccommodationContext context, RegisterViewModel model)
        {
            User user = _register.GetNewUser(model.Username, model.Password);
            UserData data = new UserData()
            {
                Email = model.Email,
                CompanyName = model.CompanyName,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            Address address = new Address()
            {
                City = model.City,
                Street = model.Street,
                LocalNumber = model.LocalNumber,
                PostalCode = model.PostalCode
            };
            await _register.SaveUserAsync(context,user, data, address);
        }
    }
}