using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    /// <summary>
    /// Model dla miejsca
    /// </summary>
    public class Place : Entity
    {

        /// <summary>
        /// Nazwa miejsca
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Adres miejsca
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Id adresu w tabeli Addresses
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Lista pokoi w danym miejscu
        /// </summary>
        public virtual IList<Room> Rooms { get; set; }
    }
}
