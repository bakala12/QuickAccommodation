using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class Offer : Entity
    {
        public OfferInfo OfferInfo { get; set; }
        public int OfferInfoId { get; set; }
        public int VendorId { get; set; }
        [InverseProperty("PurchasedOffers")]
        public virtual User Vendor { get; set; }
        public int? CutomerId { get; set; }
        [InverseProperty("AvailableOffers")]
        public virtual User Customer { get; set; }
    }
}
