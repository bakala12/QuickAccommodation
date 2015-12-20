using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class Rank : Entity
    {
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; } 
    }
}
