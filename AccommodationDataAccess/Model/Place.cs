using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class Place : Entity
    {
        public string PlaceName { get; set; }

        public virtual Address Address { get; set; }
        public int AddressId { get; set; }

       // public virtual IList<Offer> Offers { get; set; }

        //modyfikacja
        public virtual IList<Room> Rooms { get; set; }
    }
}
