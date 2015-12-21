using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    /// <summary>
    /// Model dla pokoju
    /// </summary>
    public class Room : Entity
    {
        /// <summary>
        /// Pojemność pokoju
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Numer pokoju
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Miejsce, w którym znajduje się dany pokój
        /// </summary>
        [InverseProperty("Rooms")]
        public virtual Place Place { get; set; }

        /// <summary>
        /// Id pokoju w tabeli Rooms
        /// </summary>
        public int PlaceId { get; set; }
    }
}
