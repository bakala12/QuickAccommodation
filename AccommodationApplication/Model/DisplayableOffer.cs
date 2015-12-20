using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication.Model
{
    /// <summary>
    /// Klasa do wyświetlania dostępnych ofert
    /// </summary>
    public class DisplayableOffer
    {
        public DisplayableOffer(Offer offer)
        {
            OfferInfo offerInfo = offer.OfferInfo;
            OfferStartTime = offerInfo.OfferStartTime.Date.ToString("dd/MM/yyyy");
            OfferEndTime = offerInfo.OfferEndTime.Date.ToString("dd/MM/yyyy");
            OfferEndTimeDate = offerInfo.OfferEndTime;
            OfferStartTimeDate = offerInfo.OfferStartTime;
            Address = offer.Room.Place.Address;
            PlaceName = offer.Room.Place.PlaceName;
            AvailableVacanciesNumber = offerInfo.AvailableVacanciesNumber;
            Price = offerInfo.Price;
            Description = offerInfo.Description;
            OfferPublishTime = offerInfo.OfferPublishTime;
            Id = offer.Id;
            IsBooked = offer.IsBooked;
        }
        public int Id { get; set; }

        /// <summary>
        /// Data początkowa oferty jako napis
        /// </summary>
        public string OfferStartTime { get; set; }

        /// <summary>
        /// Data końcowa oferty jako napis
        /// </summary>
        public string OfferEndTime { get; set; }

        /// <summary>
        /// Data początkowa oferty jako DateTime
        /// </summary>
        public DateTime OfferStartTimeDate { get; set; }

        /// <summary>
        /// Data końcowa oferty jako DateTime
        /// </summary>
        public DateTime OfferEndTimeDate { get; set; }

        /// <summary>
        /// Adres oferty
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Liczba wolnych miejsc
        /// </summary>
        public int AvailableVacanciesNumber { get; set; }

        /// <summary>
        /// Cena
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Opis oferty
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Data dodania oferty
        /// </summary>
        public DateTime OfferPublishTime { get; set; }

        /// <summary>
        /// Nazwa miejsca, którego dotyczy oferta
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Zwraca informację czy oferta jest jeszcze dostępna
        /// </summary>
        public bool IsBooked { get; set; }
    }
}
