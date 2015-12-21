﻿using AccommodationApplication.Commands;
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
using AccomodationWebApi;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// ViewModel dla widoku historii ofert
    /// </summary>
    public class HistoryViewModel : ViewModelBase, IPageViewModel
    {
        /// <summary>
        /// Komenda do usuwania oferty
        /// </summary>
        public ICommand RemoveCommand { get; set; }

        private readonly OffersProxy offersProxy;
        private readonly OfferInfoesProxy offerInfoesProxy;
        private readonly PlacesProxy PlacesProxy;
        private readonly AddressesProxy addressesProxy;
        private readonly UsersProxy usersProxy;
        private readonly RoomsProxy _roomsProxy = new RoomsProxy();

        public HistoryViewModel()
        {
            this.offersProxy = new OffersProxy();
            this.offerInfoesProxy = new OfferInfoesProxy();
            this.PlacesProxy = new PlacesProxy();
            this.addressesProxy = new AddressesProxy();
            this.usersProxy = new UsersProxy();

            CurrentOffersList = null;
            (App.Current as App).Login += (x, e) => { CurrentOffersList = null; OnPropertyChanged(nameof(CurrentOffersList)); };
        }

        public string Name
        {
            get
            {
                return "Historia Ofert";
            }
        }

        /// <summary>
        /// Aktualnie zaznaczona oferta (do usunięcia)
        /// </summary>
        public DisplayableOffer CurrentlySelectedOffer { get; set; }


        private static void CloseWindow(Window window)
        {
            window?.Close();
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

            var list = await offersProxy.GetUserHistoricalOffers(user.Id);

            foreach (var item in list)
            {
                OfferInfo oi = await offerInfoesProxy.Get(item.OfferInfoId);
                Room r = await _roomsProxy.Get(item.RoomId);
                Place p = await PlacesProxy.Get(r.PlaceId);
                Address a = await addressesProxy.Get(p.AddressId);

                p.Address = a;
                r.Place = p;
                item.OfferInfo = oi;
                item.Room = r;
                DisplayableOffer dof = new DisplayableOffer(item);
                ret.Add(dof);
            }

            this.CurrentOffersList = ret;

        }
    }
}