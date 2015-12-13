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
        public void Load()
        {
            var ret = new ObservableCollection<DisplayableOffer>();

            using (var context = new AccommodationContext())
            {
                //pobierz login aktualnego usera
                string currentUser = Thread.CurrentPrincipal.Identity.Name;

                //wyciągnij usera z bazy
                User user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUser));

                //lista ofert aktualnego użytkownika
                var list = user.MyOffers;

                //dla każej oferty stwórz jest wersję do wyświetlenia i dodaj do listy ofert
                foreach (var item in list)
                {
                    Offer offer = context.Offers.FirstOrDefault(x => item.Id == x.Id);
                    OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(x => x.Id == offer.OfferInfoId);
                    Place place = context.Places.FirstOrDefault(x => offer.PlaceId == x.Id);
                    Address address = context.Addresses.FirstOrDefault(x => place.AddressId == x.Id);

                    place.Address = address;
                    offer.OfferInfo = offerInfo;
                    offer.Place = place;
                    DisplayableOffer dof = new DisplayableOffer(offer);
                    ret.Add(dof);
                }
            }

            CurrentOffersList = ret;
        }

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
        public void Remove()
        {
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    //pobierz login aktualnego usera
                    string currentUser = Thread.CurrentPrincipal.Identity.Name;

                    //wyciągnij go z bazy
                    User user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUser));

                    //znajdź ofertę do usunięcia
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == CurrentlySelectedOffer.Id);
                    if (offer == null) return;

                    //pobierz dodatkowe dane oferty do usunięcia
                    OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(x => x.Id == offer.OfferInfoId);
                    Place place = context.Places.FirstOrDefault(x => x.Id == offer.PlaceId);
                    Address address = context.Addresses.FirstOrDefault(x => x.Id == place.AddressId);

                    //usuń z bazy ofertę oraz jej dane
                    context.Offers.Remove(offer);
                    context.Places.Remove(place);
                    context.Addresses.Remove(address);
                    context.OfferInfo.Remove(offerInfo);
                    user.MyOffers.Remove(offer);

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
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
                if (currentOffersList == null) Load2();
                return currentOffersList;
            }
        }
        public async void Load2()
        {

            var ret = new ObservableCollection<DisplayableOffer>();

            string currentUser = Thread.CurrentPrincipal.Identity.Name;

            User user = await usersProxy.GetUser(currentUser);

            var list = await offersProxy.GetUserOffers(user.Id);

            foreach (var item in list)
            {
                OfferInfo oi = await offerInfoesProxy.Get(item.OfferInfoId);
                Place p = await PlacesProxy.Get(item.PlaceId);
                Address a = await addressesProxy.Get(p.AddressId);

                p.Address = a;
                item.OfferInfo = oi;
                item.Place = p;
                DisplayableOffer dof = new DisplayableOffer(item);
                ret.Add(dof);
            }


            this.CurrentOffersList = ret;

        }
    }
}
