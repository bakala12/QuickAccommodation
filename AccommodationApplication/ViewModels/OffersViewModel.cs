using AccommodationApplication.Commands;
using AccommodationApplication.Interfaces;
using AccommodationApplication.Model;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AccommodationApplication.Views.Windows;
using AccommodationApplication.Services;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// ViewModel dla widoku dostępnych ofert
    /// </summary>
    public class OffersViewModel : ViewModelBase, IPageViewModel
    {
        /// <summary>
        /// Komenda do usuwania oferty
        /// </summary>
        public ICommand RemoveCommand { get; set; }

        /// <summary>
        /// Komenda do edycji oferty
        /// </summary>
        public ICommand EditCommand { get; set; }
        private readonly OffersProxy offersProxy;
        private readonly OfferInfoesProxy offerInfoesProxy;
        private readonly PlacesProxy PlacesProxy;
        private readonly AddressesProxy addressesProxy;
        private readonly UsersProxy usersProxy;

        public OffersViewModel()
        {
            RemoveCommand = new DelegateCommand(async x => await RemoveAsync());
            EditCommand = new DelegateCommand(x => Edit());

            this.offersProxy = new OffersProxy();
            this.offerInfoesProxy = new OfferInfoesProxy();
            this.PlacesProxy = new PlacesProxy();
            this.addressesProxy = new AddressesProxy();
            this.usersProxy = new UsersProxy();

            CurrentOffersList = null;
            (App.Current as App).Login += (x, e) => { CurrentOffersList = null; OnPropertyChanged(nameof(CurrentOffersList)); };
        }

        /// <summary>
        /// Uaktualnia bieżącą listę ofert użytkownika
        /// </summary>

        public string Name
        {
            get
            {
                return "Moje oferty";

            }
        }

        /// <summary>
        /// Aktualnie zaznaczona oferta (do edycji lub usunięcia)
        /// </summary>
        public DisplayableOffer CurrentlySelectedOffer { get; set; }

        /// <summary>
        /// Asynchroniczne usuwanie oferty
        /// </summary>
        /// <returns></returns>
        public async virtual Task RemoveAsync()
        {
            await Task.Run(() => Remove());
        }

        /// <summary>
        /// Funkcja do edytowania zaznaczonej oferty
        /// </summary>
        public void Edit()
        {
            EditOfferViewModel eo = new EditOfferViewModel(CurrentlySelectedOffer, this);
            EditWindow e = new EditWindow();
            eo.RequestClose += (x, ev) => CloseWindow(e);
            e.DataContext = eo;

            //pokaż nowe okno dialogowe do edycji oferty
            e.ShowDialog();
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }

        /// <summary>
        /// Funkcja do usuwania zaznaczonej oferty
        /// </summary>
        public async void Remove()
        {
            string currentUser = Thread.CurrentPrincipal.Identity.Name;
            await offersProxy.RemoveOfferAsync(currentUser, CurrentlySelectedOffer.Id);
            //uaktualnij bieżącą listę ofert
            Load();
        }

        public ObservableCollection<DisplayableOffer> currentOffersList;

        /// <summary>
        /// Lista ofert dodanych przez aktualnego usera
        /// </summary>
        public ObservableCollection<DisplayableOffer> CurrentOffersList
        {
            set
            {
                currentOffersList = value;
                OnPropertyChanged("CurrentOffersList");
            }
            get
            {
                //przy pierwszej próbie wyświetlenia pobierz listę z bazy
                if (currentOffersList == null) this.Load();
                return currentOffersList;
            }
        }
        public async void Load()
        {

            var ret = new ObservableCollection<DisplayableOffer>();

            string currentUser = Thread.CurrentPrincipal.Identity.Name;

            User user = await usersProxy.GetUser(currentUser);

            var list = await offersProxy.GetUserOffers(user.Id);

            foreach (var item in list)
            {
                OfferInfo oi = await offerInfoesProxy.Get(item.OfferInfoId);
                Place p = await PlacesProxy.Get(item.Room.PlaceId);
                Address a = await addressesProxy.Get(p.AddressId);

                p.Address = a;
                item.OfferInfo = oi;
                item.Room.Place = p;
                DisplayableOffer dof = new DisplayableOffer(item);
                ret.Add(dof);
            }

            this.CurrentOffersList = ret;

        }
    }
}
