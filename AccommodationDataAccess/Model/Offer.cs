using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    /// <summary>
    /// Model dla oferty
    /// </summary>
    public class Offer : Entity
    {
        /// <summary>
        /// Informacje dotyczące oferty
        /// </summary>
        public OfferInfo OfferInfo { get; set; }
        public int OfferInfoId { get; set; }

        /// <summary>
        /// Użytkownik, który dodał ofertę
        /// </summary>
        [InverseProperty("MyOffers")]
        public virtual User Vendor { get; set; }
        public int VendorId { get; set; }
        
        /// <summary>
        /// Użytkownik, który skorzystał z oferty
        /// </summary>
        [InverseProperty("PurchasedOffers")]
        public virtual User Customer { get; set; }
        public int? CustomerId { get; set; }

        /// <summary>
        /// Informacja o tym, czy oferta jest już zarezerwowana
        /// </summary>
        public bool IsBooked { get; set; }

        public virtual Room Room { get; set; }
        public int RoomId { get; set; }
    }

}
