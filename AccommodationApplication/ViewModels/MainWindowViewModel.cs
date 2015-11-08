using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Login;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using MahApps.Metro.Controls.Dialogs;
using AccommodationApplication.Interfaces;

namespace AccommodationApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            LoginCommand = new DelegateCommand(x => Login());
            RegisterCommand = new DelegateCommand(x => Register());


            PageViewModels.Add(new OffersViewModel());
            PageViewModels.Add(new SearchingViewModel());

            CurrentPageViewModel = PageViewModels[0];
        }

        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

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
            LoginWindow login = new LoginWindow();
            LoginWindowViewModel vm = new LoginWindowViewModel();
            vm.RequestClose += (x, e) => CloseWindow(login);
            login.DataContext = vm;
            login.ShowDialog();
        }

        protected virtual void Register()
        {
            RegisterWindow registerWindow = new RegisterWindow();
            RegiserNewUserViewModel vm = new RegiserNewUserViewModel();
            vm.RequestClose += (x, e) => CloseWindow(registerWindow);
            registerWindow.DataContext = vm;
            registerWindow.ShowDialog();
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }

        public ObservableCollection<DisplayableUser> Users
        {
            get
            {
                var ret = new ObservableCollection<DisplayableUser>();
                using (var db = new AccommodationContext())
                {
                    foreach (var user in db.Users)
                    {
                        ret.Add(new DisplayableUser(user, user.UserData));
                    }
                }
                return ret;
            }
        }

        public ObservableCollection<DisplayableOffer> offersList = new ObservableCollection<DisplayableOffer>();



        public ObservableCollection<DisplayableOffer> OffersList
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


        public class DisplayableUser
        {
            public DisplayableUser(User user, UserData data)
            {
                Id = user.Id;
                Login = user.Username;
                FirstName = data.FirstName;
                CompanyName = data.CompanyName;
            }

            public int Id { get; set; }
            public string Login { get; set; }
            public string FirstName { get; set; }
            public string CompanyName { get; set; }
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
