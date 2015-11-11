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
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;
using AccommodationApplication.Interfaces;

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
            PageViewModels.Add(new OffersViewModel());
            PageViewModels.Add(new SearchingViewModel());
            PageViewModels.Add(new AddNewOfferViewModel());

            CurrentPageViewModel = PageViewModels[0];
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get;private set; }
        public ICommand LogoutCommand { get; private set; }

        private ICommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new DelegateCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }


        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

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
        public ObservableCollection<DisplayableOffer> currentOffersList = new ObservableCollection<DisplayableOffer>();



        public ObservableCollection<DisplayableOffer> CurrentOffersList
        {
            get
            {
                var ret = new ObservableCollection<DisplayableOffer>();

                Address address = new Address()
                {
                    City = "Gołąb",
                    Street = "Piaskowa",
                    LocalNumber = "20",
                    PostalCode = "24-100"
                };
                OfferInfo offer = new OfferInfo()
                {
                    Address = address,
                    OfferStartTime = new DateTime(2015, 10, 10),
                    OfferEndTime = new DateTime(2015, 10, 11),
                    Description = "Oferta",
                    Price = 1245.55,
                    AvailableVacanciesNumber = 3,
                };
                DisplayableOffer u = new DisplayableOffer(offer);
               
                ret.Add(u);
                ret.Add(u);
                ret.Add(u);
                return ret;
            }
        }

        public class DisplayableOffer
        {
            public DisplayableOffer(OfferInfo offerInfo)
            {
                OfferStartTime = offerInfo.OfferStartTime;
                OfferEndTime = offerInfo.OfferEndTime;
                Address = offerInfo.Address;
                AvailableVacanciesNumber = offerInfo.AvailableVacanciesNumber;
                Price = offerInfo.Price;
            }
            public DateTime OfferStartTime { get; set; }
            public DateTime OfferEndTime { get; set; }
            public virtual Address Address { get; set; }
            public int AvailableVacanciesNumber { get; set; }
            public double Price { get; set; }
        }
    }
}
