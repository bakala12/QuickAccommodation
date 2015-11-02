using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;
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

        public RegiserNewUserViewModel()
        {
            RegisterCommand = new DelegateCommand(x => Register());
        }

        public ICommand RegisterCommand { get; }

        public virtual void Register()
        {
            
        }

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string CompanyName
        {
            get
            {
                return _companyName;
            }

            set
            {
                _companyName = value;
                OnPropertyChanged();
            }
        }

        public string Street
        {
            get
            {
                return _street;
            }

            set
            {
                _street = value;
                OnPropertyChanged();
            }
        }

        public string LocaleNumber
        {
            get
            {
                return _localeNumber;
            }

            set
            {
                _localeNumber = value;
                OnPropertyChanged();
            }
        }

        public string PostalCode
        {
            get
            {
                return _postalCode;
            }

            set
            {
                _postalCode = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get
            {
                return _city;
            }

            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }
    }
}
