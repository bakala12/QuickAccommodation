using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    /// <summary>
    /// Model do przechowywania adresu
    /// </summary>
    public class Address : Entity
    {
        /// <summary>
        /// Ulica
        /// </summary>
        public string Street { get; set; }
        
        /// <summary>
        /// Numer domu
        /// </summary>
        public string LocalNumber { get; set; }

        /// <summary>
        /// Kod pocztowy
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Miasto
        /// </summary>
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
