using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Interfaces;
using AccommodationApplication.Model;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// ViewModel odpowiedzialny za prezentację ofert użytkownika
    /// </summary>
    public class PurchasedOffersViewModel : ViewModelBase, IPageViewModel
    {
        /// <summary>
        /// Nazwa ViewModelu, zaimplementowane z IPageViewModel
        /// </summary>
        public string Name => "Zarezerwowane oferty";

        /// <summary>
        /// Inicjalizuje nową instancje ViewModelu
        /// </summary>
        public PurchasedOffersViewModel()
        {
            _purchasedOffers = null;
            (App.Current as App).Login += (x, e) => OnPropertyChanged(nameof(PurchasedOffers));
        }

        private ObservableCollection<DisplayableOfferViewModel> _purchasedOffers;

        /// <summary>
        /// Zwraca kolekcję ViewModeli zakupionych ofert
        /// </summary>
        public ObservableCollection<DisplayableOfferViewModel> PurchasedOffers
        {
            get
            {
                if(_purchasedOffers==null) Load();
                return _purchasedOffers;
            }
            set
            {
                _purchasedOffers = value;
                OnPropertyChanged();
            }
        }
        
        private void Load()
        {
            var coll=new ObservableCollection<DisplayableOfferViewModel>();
            string username = Thread.CurrentPrincipal.Identity.Name;
            using (var context = new AccommodationContext())
            {
                User u = context.Users.FirstOrDefault(us => us.Username.Equals(username));
                if (u == null) throw new Exception();
                IEnumerable<Offer> offers =
                    context.Offers.Where(o => o.CustomerId == u.Id)
                        .Include(o => o.OfferInfo)
                        .Include(o => o.Room.Place)
                        .Include(o => o.Room.Place.Address);
                foreach (var offer in offers)
                {
                    coll.Add(new DisplayableOfferViewModel(new DisplayableOffer(offer)));
                }
            }
            PurchasedOffers = coll;
        }
    }
}
