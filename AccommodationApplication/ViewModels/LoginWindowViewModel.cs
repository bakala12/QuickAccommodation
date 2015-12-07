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
using AccommodationApplication.Services;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// Reprezentuję instancję ViewModelu odpowiedzialną za logowanie uzytkownika do aplikacji
    /// </summary>
    public class LoginWindowViewModel : CloseableViewModel
    {
        private string _username;
        private string _errorText;

        /// <summary>
        /// Inicjalizuje nowa instancję klasy LoginWindowViewModel
        /// </summary>
        public LoginWindowViewModel()
        {
            LoginCommand = new DelegateCommand(async x => await LoginAsync(x));
        }

        /// <summary>
        /// Pobiera lub ustawia nazwę użytkownika
        /// </summary>
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia tekst wiadmości z błędem
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Komenda reagująca na zlecenie zalogowania
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Asynchrincznie uwierzytelnia i loguje użytkownika do aplikacji 
        /// </summary>
        /// <param name="parameter">Parametr komendy</param>
        /// <returns></returns>
        public virtual async Task LoginAsync(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            if(passwordBox==null)
                throw new InvalidOperationException();
            //Login operation here
            CustomPrincipal principal=Thread.CurrentPrincipal as CustomPrincipal;
            if(principal==null)
                throw new InvalidOperationException();
            LoginProxy _service = new LoginProxy();
            CustomIdentity identity = await _service.GetUserAsync(Username, passwordBox.Password);
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
