using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Services;
using AccommodationShared.Dtos;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    public class EditUserDataViewModel : CloseableViewModel
    {
        private string _firstName;
        private string _lastName;
        private string _companyName;
        private string _email;
        private readonly string _username;
        private readonly UserProfileProxy _service;

        public EditUserDataViewModel(string username)
        {
            _username = username;
            _service = new UserProfileProxy();
            SaveDataCommand = new DelegateCommand(async x=>await SaveDataAsync());
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

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveDataCommand { get; }

        protected async Task SaveDataAsync()
        {
            if (!ValidateData()) return;
            UserBasicDataDto dto = new UserBasicDataDto()
            {
                FirstName=FirstName,
                LastName = LastName,
                Email = Email,
                CompanyName = CompanyName
            };
            await _service.ChangeUserDataAsync(_username, dto);
            Close();
        }

        private bool ValidateData()
        {
            UserCredentialsValidator validator = new UserCredentialsValidator();
            if (!validator.ValidateName(FirstName))
            {
                Error = "Imię nie może byc puste";
                return false;
            }
            if (!validator.ValidateName(LastName))
            {
                Error = "Nazwisko nie może być puste";
                return false;
            }
            string message;
            if (!validator.ValidateEmail(Email, out message))
            {
                Error = message;
                return false;
            }
            return true;
        }

        private string _error;
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
