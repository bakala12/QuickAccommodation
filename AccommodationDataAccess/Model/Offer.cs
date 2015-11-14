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
    public class Offer : Entity
    {
        [InverseProperty("Offers")]
        public virtual Place Place { get; set; }
        public int PlaceId { get; set; }

        public OfferInfo OfferInfo { get; set; }
        public int OfferInfoId { get; set; }

        [InverseProperty("MyOffers")]
        public virtual User Vendor { get; set; }
        public int VendorId { get; set; }
        
        [InverseProperty("PurchasedOffers")]
        public virtual User Customer { get; set; }
        public int? CustomerId { get; set; }
    }

}
