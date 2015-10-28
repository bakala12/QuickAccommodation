using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Media.TextFormatting;
using AccommodationApplication.Commands;

namespace AccommodationApplication.ViewModels
{
    public class RegiserNewUserViewModel : LoginWindowViewModel, IDataErrorInfo
    {
        private string _password2;
        private string _email;

        public RegiserNewUserViewModel()
        {
            RegisterCommand=new DelegateCommand(x=>Register());
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
                        if (string.IsNullOrEmpty(Login) || Login.Length < 6)
                            return "Login musi zawierać przynajmniej 6 znaków";
                        //Weryfikacja czy login jest już zajęty
                        break;
                    case "Password":
                        if (string.IsNullOrEmpty(Password))
                            return "Hasło nie może być puste";
                        if (Password.Length < 8)
                            return "Hasło musi zawierać przynajmniej 8 znaków";
                        if (Password.Count(char.IsDigit) < 2)
                            return "Hasło musi zawierać przynajmniej 2 cyfry";
                        break;
                    case "Password2":
                        if (string.IsNullOrEmpty(Password2) || !Password2.Equals(Password))
                            return "Hasła nie są zgodne";
                        break;
                    case "Email":
                        if (Email==null || !Regex.IsMatch(Email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                            return "Niepoprawny adres email";
                        break;
                    default:
                        break;
                }
                return string.Empty;
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
