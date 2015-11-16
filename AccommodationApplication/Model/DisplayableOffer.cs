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
            OfferStartTime = offerInfo.OfferStartTime.Date.ToString("dd/MM/yyyy");
            OfferEndTime = offerInfo.OfferEndTime.Date.ToString("dd/MM/yyyy");
            OfferEndTimeDate = offerInfo.OfferEndTime;
            OfferStartTimeDate = offerInfo.OfferStartTime;
            Address = offer.Place.Address;
            PlaceName = offer.Place.PlaceName;
            AvailableVacanciesNumber = offerInfo.AvailableVacanciesNumber;
            Price = offerInfo.Price;
            Description = offerInfo.Description;
            OfferPublishTime = offerInfo.OfferPublishTime;
            Id = offer.Id;
        }
        public int Id { get; set; }
        public string OfferStartTime { get; set; }
        public string OfferEndTime { get; set; }
        public DateTime OfferStartTimeDate { get; set; }
        public DateTime OfferEndTimeDate { get; set; }

        public virtual Address Address { get; set; }
        public int AvailableVacanciesNumber { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime OfferPublishTime { get; set; }
        public string PlaceName { get; set; }
    }
}
