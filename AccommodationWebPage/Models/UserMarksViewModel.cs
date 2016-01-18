using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    public class UserMarksViewModel
    {

        public UserMarksViewModel(HistoricalOffer offer, User user)
        {
            OfferInfo offerInfo = offer.OfferInfo;
            OfferStartTime = offerInfo.OfferStartTime.Date.ToString("dd/MM/yyyy");
            OfferEndTime = offerInfo.OfferEndTime.Date.ToString("dd/MM/yyyy");
            OfferEndTimeDate = offerInfo.OfferEndTime;
            OfferStartTimeDate = offerInfo.OfferStartTime;
            PlaceName = offer.Room.Place.PlaceName;
            RoomNumber = offer.Room.Number;
            Username = user.Username;
            ReservedOfferId = offer.Id;
        }

        /// <summary>
        /// Id oferty
        /// </summary>
        public int ReservedOfferId { get; set; }

        /// <summary>
        /// Nazwa użytkownika
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Id oferty
        /// </summary>
        public int UserId { get; set; }

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
        /// Nazwa miejsca, którego dotyczy oferta
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Numer pokoju.
        /// </summary>
        public string RoomNumber { get; set; }

    }
}