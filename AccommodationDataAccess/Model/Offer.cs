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
        public DateTime OfferStartTime { get; set; }
        public DateTime OfferEndTime { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string Description { get; set; }
        public int AvailableVacanciesNumber { get; set; }
        public SqlMoney Price { get; set; }
        //obrazek???
        public int VendorId { get; set; }
        [ForeignKey("")]
        public virtual LoggedUser Vendor { get; set; }
        public int? CutomerId { get; set; }
        [ForeignKey("")]
        public virtual LoggedUser Customer { get; set; }
    }
}
