using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Converter;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    public class RegiserNewUserViewModel : CloseableViewModel
    {
        private string _username;
        private string _email;

        private string _firstName;
        private string _lastName;
        private string _companyName;

        private string _street;
        private string _localeNumber;
        private string _postalCode;
        private string _city;

        private string _error;
        private readonly IUserCredentialsValidator _validator;
        private readonly IRegisterUser _register;

        public RegiserNewUserViewModel(IUserCredentialsValidator validator, IRegisterUser register)
        {
            if(validator==null || register==null) throw new ArgumentNullException();
            _validator = validator;
            _register = register;
            RegisterCommand = new DelegateCommand(async x=>await RegisterAsync(x as PasswordBox[]));
        }

        public ICommand RegisterCommand { get; }

        public virtual async Task RegisterAsync(PasswordBox[] passwords)
        {
            if(passwords==null || passwords.Length!=2)
                throw new InvalidOperationException();
            //Validation
            Error = string.Empty;
            string reason;
            if (string.IsNullOrEmpty(Username))
            {
                Error = "Login nie może być pusty";
            }
            if (!_validator.ValidateEmail(Email))
            {
                Error = "Nieprawidłowy adres email";
                return;
            }
            if (!_validator.ValidatePassword(passwords[0].Password, out reason))
            {
                Error = reason;
                return;
            }
            if (!_validator.ValidatePasswordConfirmation(passwords[0].Password, passwords[1].Password))
            {
                Error = "Hasła nie są zgodne";
                return;
            }
            bool b = await _validator.ValidateUsernameAsync<AccommodationContext>(Username);
            if (!b)
            {
                Error = "Login jest już zajęty";
                return;
            }
            // Imie nazwisko
            if (string.IsNullOrEmpty(FirstName))
            {
                Error = "Należy podać imię";
                return;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                Error = "Należy podać nazwisko";
                return;
            }
            //Register logic here
            User user = _register.GetNewUser(Username, passwords[0].Password);
            UserData userData= new UserData();
            userData.FirstName = FirstName;
            userData.LastName = LastName;
            userData.Email = Email;
            userData.CompanyName = CompanyName ?? string.Empty;
            Address address = new Address();
            //Address validation and setting...
            await _register.SaveUserAsync<AccommodationContext>(user, userData, address);
            Close();
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                OnPropertyChanged();
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged();
            }
        }

        public string LocaleNumber
        {
            get { return _localeNumber; }
            set
            {
                _localeNumber = value;
                OnPropertyChanged();
            }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }
    }
}
