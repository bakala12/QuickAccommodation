using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Annotations;
using AccommodationApplication.Commands;
using AccommodationApplication.Login;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using MahApps.Metro.Controls.Dialogs;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _authenticatedUser;

        public MainWindowViewModel()
        {
            LoginCommand = new DelegateCommand(x => Login());
            RegisterCommand = new DelegateCommand(x=>Register());
            LogoutCommand = new DelegateCommand(x=>Logout());
            AuthenticatedUser = null;
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get;private set; }
        public ICommand LogoutCommand { get; private set; }

        protected virtual void Login()
        {
            LoginWindow login=new LoginWindow();
            LoginWindowViewModel vm=new LoginWindowViewModel(new UserAuthenticationService());
            vm.RequestClose += (x,e)=>CloseWindow(login);
            login.DataContext = vm;
            login.ShowDialog();
            AuthenticatedUser = Thread.CurrentPrincipal.Identity is AnonymousIdentity?null:Thread.CurrentPrincipal.Identity.Name;
        }

        protected virtual void Register()
        {
            RegisterWindow registerWindow = new RegisterWindow();
            RegisterUserViewModel vm = new RegisterUserViewModel(new UserCredentialsValidator(), new UserRegister());
            vm.RequestClose += (x, e) => CloseWindow(registerWindow);
            registerWindow.DataContext = vm;
            registerWindow.ShowDialog();
        }

        private void Logout()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if(principal==null) throw new InvalidOperationException();
            principal.Identity=new AnonymousIdentity();
            AuthenticatedUser = null;
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }

        public bool IsAuthenticated => AuthenticatedUser != null;

        public string AuthenticatedUser
        {
            get { return _authenticatedUser; }
            set
            {
                _authenticatedUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }
    }
}
