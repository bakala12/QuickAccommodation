using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class PurchasedOffer : Entity
    {
        public AvailableOffer AvailableOffer { get; set; }
        public int AvailableOfferId { get; set; }

        //[InverseProperty("PurchasedOffers")]
        //public virtual User Customer { get; set; }
        //public int CustomerId { get; set; }
    }

}
