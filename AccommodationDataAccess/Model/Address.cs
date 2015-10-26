using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class Address : Entity
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string LocalNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
