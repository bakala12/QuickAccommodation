using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class OfferInfo : Entity
    {
        public DateTime OfferStartTime { get; set; }
        public DateTime OfferEndTime { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string Description { get; set; }
        public int AvailableVacanciesNumber { get; set; }
        public double Price { get; set; }
        public DateTime OfferPublishTime { get; set; }
    }
}
