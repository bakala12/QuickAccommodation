using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AccommodationApplication.Commands;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    public class LoginWindowViewModel : CloseableViewModel
    {
        private string _username;
        private string _errorText;
        private readonly string _errorMessage = "Nieprawidłowa nazwa użytkownika lub hasło!";

        public LoginWindowViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
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

        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public virtual void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            if(passwordBox==null)
                throw new InvalidOperationException();
            //Login operation here
            ErrorText = passwordBox.Password;
        }
    }
}
