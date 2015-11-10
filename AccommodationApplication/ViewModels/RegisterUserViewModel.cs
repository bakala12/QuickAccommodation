using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;

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

        public RegisterUserViewModel()
        {
            NextCommand = new DelegateCommand(x => NextScreen());
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

        private void NextScreen()
        {
            switch (CurrentScreen)
            {
                case CurrentScreen.Credentials:
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
