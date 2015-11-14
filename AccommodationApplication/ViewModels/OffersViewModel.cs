using AccommodationApplication.Interfaces;
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

namespace AccommodationApplication.ViewModels
{
    public class OffersViewModel : IPageViewModel
    {
        public string Name
        {
            get
            {
                return "My offers";
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
                Description = offerInfo.Description;
                OfferPublishTime = offerInfo.OfferPublishTime;

            }
            public DateTime OfferStartTime { get; set; }
            public DateTime OfferEndTime { get; set; }
            public virtual Address Address { get; set; }
            public int AvailableVacanciesNumber { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public DateTime OfferPublishTime { get; set; }
        }

        public ObservableCollection<DisplayableOffer> currentOffersList = new ObservableCollection<DisplayableOffer>();

        public ObservableCollection<DisplayableOffer> CurrentOffersList
        {
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
                        ret.Add(new DisplayableOffer(context.OfferInfo.FirstOrDefault(x => x.Id == item.Id)));
                    }
                    return ret;
                }
            }
        }
    }
}
