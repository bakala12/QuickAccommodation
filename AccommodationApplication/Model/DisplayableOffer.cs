using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication.Model
{
    public class DisplayableOffer
    {
        public DisplayableOffer(Offer offer)
        {
            OfferInfo offerInfo = offer.OfferInfo;
            OfferStartTime = offerInfo.OfferStartTime;
            OfferEndTime = offerInfo.OfferEndTime;
            Address = offer.Place.Address;
            AvailableVacanciesNumber = offerInfo.AvailableVacanciesNumber;
            Price = offerInfo.Price;
            Description = offerInfo.Description;
            OfferPublishTime = offerInfo.OfferPublishTime;
        }

        public DisplayableOffer(OfferInfo offerInfo, Address address)
        {
            OfferStartTime = offerInfo.OfferStartTime;
            OfferEndTime = offerInfo.OfferEndTime;
            Address = address;
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
}
