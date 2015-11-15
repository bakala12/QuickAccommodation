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
            Address = offer.Place.Address;
            AvailableVacanciesNumber = offerInfo.AvailableVacanciesNumber;
            Price = offerInfo.Price;
            Description = offerInfo.Description;
            OfferPublishTime = offerInfo.OfferPublishTime;
            Id = offer.Id;
        }
        public int Id;
        public string OfferStartTime { get; set; }
        public string OfferEndTime { get; set; }
        public virtual Address Address { get; set; }
        public int AvailableVacanciesNumber { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime OfferPublishTime { get; set; }
    }
}
