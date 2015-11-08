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

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(Street);
            s.Append(" ");
            s.Append(LocalNumber);
            s.Append(" ");
            s.Append(PostalCode);
            s.Append(" ");
            s.Append(City);
            return s.ToString();
        }
    }
}
