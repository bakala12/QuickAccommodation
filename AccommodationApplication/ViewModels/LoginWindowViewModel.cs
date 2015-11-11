using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationDataAccess.Domain;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;

namespace AccommodationApplication.ViewModels
{
    public class LoginWindowViewModel : CloseableViewModel
    {
        private string _username;
        private string _errorText;
        private readonly IUserAuthenticationService _authenticationService;

        public LoginWindowViewModel(IUserAuthenticationService authenticationService)
        {
            if(authenticationService==null) throw new ArgumentNullException();
            _authenticationService = authenticationService;
            LoginCommand = new DelegateCommand(async x => await LoginAsync(x));
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

        public virtual async Task LoginAsync(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            if(passwordBox==null)
                throw new InvalidOperationException();
            //Login operation here
            CustomPrincipal principal=Thread.CurrentPrincipal as CustomPrincipal;
            if(principal==null)
                throw new InvalidOperationException();
            CustomIdentity identity =
                await _authenticationService.AuthenticateUserAsync<AccommodationContext>(Username, passwordBox.Password);
            if (identity == null)
            {
                ErrorText = "Nieprawidłowa nazwa użytkownika lub hasło";
                return;
            }
            principal.Identity = identity;
            Close();
        }
    }
}
