using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;

namespace AccommodationApplication.ViewModels
{
    public class LoginWindowViewModel : CloseableViewModel
    {
        private string _login;
        private string _password;

        public LoginWindowViewModel()
        {
            CloseCommand = new DelegateCommand(x=>Close());
            LoginCommand = new DelegateCommand(x=>LogIn(), x=>CanLogIn());
        }

        public ICommand CloseCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }

        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        protected virtual void LogIn()
        {
            //Login logic here
            Close();
        }

        protected virtual bool CanLogIn()
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if(propertyName==null || propertyName.Equals(nameof(Login)) || propertyName.Equals(nameof(Password)))
                LoginCommand.RaiseCanExecuteChangedEvent();
            base.OnPropertyChanged(propertyName);
        }
    }
}
