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
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;
using AccommodationApplication.Interfaces;
using System.Windows.Controls;
using AccommodationApplication.Views.Windows;
using LoginWindow = AccommodationApplication.Views.Windows.LoginWindow;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// Reprezentuje ViewModel dla głównego okna aplikacji
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private string _authenticatedUser;

        /// <summary>
        /// Tworzy nową instancję klasy MainWindowViewModel
        /// </summary>
        public MainWindowViewModel()
        {
            LoginCommand = new DelegateCommand(x => Login());
            RegisterCommand = new DelegateCommand(x => Register());
            LogoutCommand = new DelegateCommand(x => Logout());
            ChangePageCommand = new DelegateCommand(async
                       p => await ChangePageAsync((IPageViewModel)p),
                        p => p is IPageViewModel);
            AuthenticatedUser = null;
            PageViewModels.Add(new OffersViewModel());
            PageViewModels.Add(new SearchingViewModel());
            PageViewModels.Add(new AddNewOfferViewModel());
            PageViewModels.Add(new PurchasedOffersViewModel());
            PageViewModels.Add(new MyProfileViewModel());
            CurrentPageViewModel = PageViewModels[0];
        }

        /// <summary>
        /// Komenda logowania
        /// </summary>
        public ICommand LoginCommand { get; private set; }
        /// <summary>
        /// Komenda rejestracji nowego uzytkownika
        /// </summary>
        public ICommand RegisterCommand { get; private set; }
        /// <summary>
        /// Komenda logowania
        /// </summary>
        public ICommand LogoutCommand { get; private set; }
        /// <summary>
        /// Komenda zmiany aktualnie wyświetlanego widoku
        /// </summary>
        public ICommand ChangePageCommand { get; }

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        /// <summary>
        /// Asynchronicznie zmienia aktualnie wyświetlaną stronę
        /// </summary>
        /// <param name="p">Nowa strona do wyświetlenia</param>
        /// <returns></returns>
        public async virtual Task ChangePageAsync(IPageViewModel p)
        {
            await Task.Run(() => ChangeViewModel(p));
        }

        /// <summary>
        /// Aktualna lista stron
        /// </summary>
        public List<IPageViewModel> PageViewModels => _pageViewModels ?? (_pageViewModels = new List<IPageViewModel>());

        /// <summary>
        /// ViewModel aktualnie wyświetlanej strony
        /// </summary>
        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Zmienia obecnie wyświetlaną stronę na podaną
        /// </summary>
        /// <param name="viewModel">ViewModel aktualnie wyświetlanej strony</param>
        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        /// <summary>
        /// Wyświetla okno logowania użytkownika do aplikacji
        /// </summary>
        protected virtual void Login()
        {
            LoginWindow login = new LoginWindow();
            LoginWindowViewModel vm = new LoginWindowViewModel();
            vm.RequestClose += (x, e) => CloseWindow(login);
            login.DataContext = vm;
            login.ShowDialog();
            AuthenticatedUser = Thread.CurrentPrincipal.Identity is AnonymousIdentity ? null : Thread.CurrentPrincipal.Identity.Name;
            (App.Current as App)?.RaiseLoginEvent();
        }

        /// <summary>
        /// Wyświetla okno rejestracji nowego użytkownika
        /// </summary>
        protected virtual void Register()
        {
            RegisterWindow registerWindow = new RegisterWindow();
            RegisterUserViewModel vm = new RegisterUserViewModel(new UserCredentialsValidator());
            vm.RequestClose += (x, e) => CloseWindow(registerWindow);
            registerWindow.DataContext = vm;
            registerWindow.ShowDialog();
        }

        /// <summary>
        /// Wylogowuje zalogowanego użytkownika
        /// </summary>
        private void Logout()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (principal == null) throw new InvalidOperationException();
            principal.Identity = new AnonymousIdentity();
            AuthenticatedUser = null;
        }

        /// <summary>
        /// Zamyka podane okno
        /// </summary>
        /// <param name="window">Okno do zamknięcia</param>
        private static void CloseWindow(Window window)
        {
            window?.Close();
        }

        /// <summary>
        /// Informuje czy aktualnie jakiś użytkownik jest zalogowany w aplikacji
        /// </summary>
        public bool IsAuthenticated => AuthenticatedUser != null;

        /// <summary>
        /// Zwraca nazwę użytkownika dla aktualnie zalogowanego użytkownika
        /// </summary>
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
