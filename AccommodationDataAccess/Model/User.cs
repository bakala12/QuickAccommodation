using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        public int UserDataId { get; set; }
        public virtual UserData UserData { get; set; }

        public virtual IList<Offer> AvailableOffers { get; set; }
        public virtual IList<Offer> PurchasedOffers { get; set; }

        public override string ToString()
        {
            return Username.ToString();
        }
    }
}
