using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class Room : Entity
    {
        public int Capacity { get; set; }
        public string Name { get; set; }

        [InverseProperty("Rooms")]
        public virtual Place Place { get; set; }
        public int PlaceId { get; set; }
    }
}
