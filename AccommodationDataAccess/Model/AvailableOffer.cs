using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class AvailableOffer : Entity
    {
        public OfferInfo OfferInfo { get; set; }
        public int OfferInfoId { get; set; }

        [InverseProperty("MyOffers")]
        public virtual User Vendor { get; set; }
        public int VendorId { get; set; }

    }

}
