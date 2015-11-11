using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    public enum CurrentScreen
    {
        Credentials, BasicData, Address
    }

    public class RegisterUserViewModel : CloseableViewModel
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
        private CurrentScreen _currentScreen;
        private readonly IUserCredentialsValidator _validator;
        private readonly IRegisterUser _register;
        private User _user;
        private UserData _userData;
        private Address _address;

        public RegisterUserViewModel(IUserCredentialsValidator validator, IRegisterUser regiser)
        {
            if(validator == null || regiser==null) throw new ArgumentNullException();
            _validator = validator;
            _register = regiser;
            NextCommand = new DelegateCommand(async x=> await NextScreenAsync(x));
            RegisterCommand = new DelegateCommand(async x =>await RegisterAsync());
            CurrentScreen = CurrentScreen.Credentials;
        }

        public ICommand NextCommand { get; }
        public ICommand RegisterCommand { get; }

        public CurrentScreen CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                _currentScreen = value;
                OnPropertyChanged();
            }
        }

        public async Task NextScreenAsync(object x)
        {
            switch (CurrentScreen)
            {
                case CurrentScreen.Credentials:
                    PasswordBox[] passwords = x as PasswordBox[];
                    if(passwords==null || passwords.Length!=2) throw new ArgumentException();
                    string err;
                    if (string.IsNullOrEmpty(Username))
                    {
                        Error = "Podaj nazwę użytkownika";
                        return;
                    }
                    bool b = await _validator.ValidateUsernameAsync<AccommodationContext>(Username);
                    if (!b)
                    {
                        Error = "Nazwa użytkownika jest już zajęta";
                        return;
                    }
                    if (!_validator.ValidateEmail(Email, out err) || 
                        !_validator.ValidatePassword(passwords[0].Password, out err) || 
                        !_validator.ValidatePasswordConfirmation(passwords[0].Password, passwords[1].Password, out err))
                    {
                        Error = err;
                        return;
                    }
                    _user = _register.GetNewUser(Username, passwords[0].Password);
                    _userData = new UserData() {Email = Email};
                    CurrentScreen=CurrentScreen.BasicData;
                    break;
                case CurrentScreen.BasicData:
                    if (!_validator.ValidateName(FirstName))
                    {
                        Error = "Należy podać imię";
                        return;
                    }
                    if (!_validator.ValidateName(LastName))
                    {
                        Error = "Należy podać nazwisko";
                        return;
                    }
                    _userData.FirstName = FirstName;
                    _userData.LastName = LastName;
                    _userData.CompanyName = CompanyName ?? string.Empty;
                    CurrentScreen=CurrentScreen.Address;
                    break;
            }
        }

        public async virtual Task RegisterAsync()
        {
            _address = new Address() {City = City, Street = Street, PostalCode = PostalCode, LocalNumber = LocaleNumber};
            await _register.SaveUserAsync<AccommodationContext>(_user, _userData, _address);
            Close();
        }

        #region Properties

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

        #endregion
    }
}
