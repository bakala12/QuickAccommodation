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
using System.Windows.Data;
using System.Windows.Input;

namespace AccommodationApplication.ViewModels
{
    public class OffersViewModel : ViewModelBase, IPageViewModel
    {
        public ICommand RemoveCommand { get; set; }
  

        public OffersViewModel()
        {
            RemoveCommand = new DelegateCommand(async x => await RemoveAsync());

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

        public void Remove()
        {
            using (var context = new AccommodationContext())
            {
                using (var scope = new TransactionScope())
                {
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == CurrentlySelectedOffer.Id);
                    if (offer == null) return;
                    context.Offers.Remove(offer);
                    DisplayableOffer displayableOffer = CurrentOffersList.FirstOrDefault(x => x.Id == offer.Id);
                    context.SaveChanges();
                    scope.Complete();
                }
            }

        }

        public ObservableCollection<DisplayableOffer> currentOffersList = new ObservableCollection<DisplayableOffer>();

        public ObservableCollection<DisplayableOffer> CurrentOffersList
        {
            set
            {
                currentOffersList = value;
                OnPropertyChanged("CurrentOffersList");
            }
            get
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
                        OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(x => item.Id == offer.Id);
                        Place place = context.Places.FirstOrDefault(x => offer.PlaceId == x.Id);
                        Address address = context.Addresses.FirstOrDefault(x => place.AddressId == x.Id);

                        place.Address = address;
                        offer.OfferInfo = offerInfo;
                        offer.Place = place;
                        ret.Add(new DisplayableOffer(offer));
                    }
                }
                return ret;

            }
        }
    }
}
