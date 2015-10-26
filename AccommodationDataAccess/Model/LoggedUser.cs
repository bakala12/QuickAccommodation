using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class LoggedUser : Entity
    {
        public int UserDataId { get; set; }
        public virtual UserData UserData { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        //public virtual IList<Offer> AvailableOffers { get; set; }
        //public virtual IList<Offer> PurchasedOffers { get; set; }
    }
}
