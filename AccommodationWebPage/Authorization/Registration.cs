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
    internal sealed class Registration
    {
        private Registration() { }

        private static readonly object SyncRoot = new object();
        private static Registration _instance;

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

        public async Task<string> ValidateUserAsync(RegisterViewModel model)
        {
            string errorMessage;
            bool b = await _validator.ValidateUsernameAsync<AccommodationContext>(model.Username);
            if (!b) return "Nieprawidłowa nazwa użytkownika lub hasło";
            return !ValidateUserData(model, out errorMessage) ? errorMessage : string.Empty;
        }

        public async Task SaveUserAsync(RegisterViewModel model)
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
            await _register.SaveUserAsync<AccommodationContext>(user, data, address);
        }
    }
}