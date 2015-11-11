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

    public class RegisterUserViewModel : CloseableViewModel, IDataErrorInfo
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

        public RegisterUserViewModel(IUserCredentialsValidator validator, IRegisterUser regiser)
        {
            if(validator == null || regiser==null) throw new ArgumentNullException();
            _validator = validator;
            _register = regiser;
            NextCommand = new DelegateCommand(async x=> await NextScreenAsync(x));
            RegisterCommand = new DelegateCommand(x => Register());
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

        private async Task NextScreenAsync(object x)
        {
            switch (CurrentScreen)
            {
                case CurrentScreen.Credentials:
                    PasswordBox[] passwords = x as PasswordBox[];
                    if(passwords==null || passwords.Length!=2) throw new ArgumentException();
                    string err;
                    if (!_validator.ValidatePassword(passwords[0].Password, out err))
                    {
                        Error = err;
                        return;
                    }
                    if (!_validator.ValidatePasswordConfirmation(passwords[0].Password, passwords[1].Password, out err))
                    {
                        Error = err;
                        return;
                    }
                    bool b = await _validator.ValidateUsernameAsync<AccommodationContext>(Username);
                    if (!b)
                    {
                        Error = "Nazwa użytkownika jest już zajęta";
                        return;
                    }
                    User user = _register.GetNewUser(Username, passwords[0].Password);
                    CurrentScreen=CurrentScreen.BasicData;
                    break;
                case CurrentScreen.BasicData:
                    CurrentScreen=CurrentScreen.Address;
                    break;
            }
        }

        protected virtual void Register()
        {

        }

        public string this[string columnName]
        {
            get
            {
                string message;
                switch (columnName)
                {
                    case "Email":
                        return !_validator.ValidateEmail(Email, out message) ?message: string.Empty;
                }
                return string.Empty;
            }
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
