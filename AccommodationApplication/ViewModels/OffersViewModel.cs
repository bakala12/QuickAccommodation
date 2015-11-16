using AccommodationApplication.Commands;
using AccommodationApplication.Editing;
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

namespace AccommodationApplication.ViewModels
{
    public class OffersViewModel : ViewModelBase, IPageViewModel
    {
        public ICommand RemoveCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public OffersViewModel()
        {
            RemoveCommand = new DelegateCommand(async x => await RemoveAsync());
            //RemoveCommand = new DelegateCommand(x => Remove());
            EditCommand = new DelegateCommand(x => Edit());
           
        }

        public void Load()
        {
            var ret = new ObservableCollection<DisplayableOffer>();

            using (var context = new AccommodationContext())
            {
                string currentUser = Thread.CurrentPrincipal.Identity.Name;

                User user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUser));
                var list = user.MyOffers;

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


        public DisplayableOffer CurrentlySelectedOffer { get; set; }


   

        public async virtual Task RemoveAsync()
        {
            await Task.Run(() => Remove());
        }

        public void Edit()
        {
            EditOfferViewModel eo = new EditOfferViewModel(CurrentlySelectedOffer, this);
            EditWindow e = new EditWindow();
            e.DataContext = eo;
            e.ShowDialog();
        }

        public void Remove()
        {
            using (var context = new AccommodationContext())
            {
                using (var scope = new TransactionScope())
                {
                    string currentUser = Thread.CurrentPrincipal.Identity.Name;
                    User user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUser));
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == CurrentlySelectedOffer.Id);
                    if (offer == null) return;
                    OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(x => x.Id == offer.OfferInfoId);
                    Place place = context.Places.FirstOrDefault(x => x.Id == offer.PlaceId);
                    Address address = context.Addresses.FirstOrDefault(x => x.Id == place.AddressId);

                    context.Offers.Remove(offer);
                    context.Places.Remove(place);
                    context.Addresses.Remove(address);
                    context.OfferInfo.Remove(offerInfo);
                    user.MyOffers.Remove(offer);
                    
                    context.SaveChanges();
                    scope.Complete();
                }
            }
            Load();
        }

        public ObservableCollection<DisplayableOffer> currentOffersList;

        public ObservableCollection<DisplayableOffer> CurrentOffersList
        {
            set
            {
                currentOffersList = value;
                OnPropertyChanged("CurrentOffersList");
            }
            get
            {
                if (currentOffersList == null) Load();
                return currentOffersList;
            }
        }
    }
}
