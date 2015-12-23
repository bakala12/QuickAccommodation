using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class HistoricalOffer : Entity
    {
        /// <summary>
        /// Informacje dotyczące oferty
        /// </summary>
        public OfferInfo OfferInfo { get; set; }
        public int OfferInfoId { get; set; }

        /// <summary>
        /// Użytkownik, który dodał ofertę
        /// </summary>
        [InverseProperty("MyHistoricalOffers")]
        public virtual User Vendor { get; set; }
        public int VendorId { get; set; }

        /// <summary>
        /// Użytkownik, który skorzystał z oferty
        /// </summary>
        [InverseProperty("PurchasedHistoricalOffers")]
        public virtual User Customer { get; set; }
        public int? CustomerId { get; set; }

        /// <summary>
        /// Informacja o tym, czy oferta jest już zarezerwowana
        /// </summary>
        public bool IsBooked { get; set; }

        /// <summary>
        /// Informacja o pokoju.
        /// </summary>
        public virtual Room Room { get; set; }
        public int RoomId { get; set; }

        /// <summary>
        /// Powiązanie z aktualną ofertą.
        /// </summary>
        public virtual Offer OriginalOffer { get; set; }
        public int? OriginalOfferId { get; set; }
    }
}
