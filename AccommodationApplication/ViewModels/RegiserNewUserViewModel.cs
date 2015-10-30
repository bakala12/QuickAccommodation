using System;
using System.ComponentModel;
using System.Threading.Tasks;
using AccommodationApplication.Commands;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    public class RegiserNewUserViewModel : LoginWindowViewModel, IDataErrorInfo
    {
        private string _password2;
        private string _email;
        private readonly INewUserDataValidator _validator;

        public RegiserNewUserViewModel(INewUserDataValidator validator)
        {
            _validator = validator;
            RegisterCommand = new DelegateCommand(x=>Register());
        }

        public string Password2
        {
            get { return _password2; }
            set { _password2 = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public DelegateCommand RegisterCommand { get; set; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Login":
                        return
                            Task.Run(async () => await _validator.ValidateUserLoginAsync(Login)).Result.AdditionalInfo;
                    case "Password":
                        return _validator.ValidateUserPassword(Password).AdditionalInfo;
                    case "Password2":
                        return _validator.ValidateUserPasswordConfirmed(Password, Password2).AdditionalInfo;
                    case "Email":
                        return _validator.ValidateUserEmail(Email) ? string.Empty : "Email is not valid";
                    default:
                        return string.Empty;
                }
            }
        }

        public string Error { get; } = string.Empty;

        protected virtual void Register()
        {
            //Register logic here
            Close();
        }
    }
}
