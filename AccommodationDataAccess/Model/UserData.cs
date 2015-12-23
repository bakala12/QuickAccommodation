using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class UserData : Entity
    {
        /// <summary>
        /// Imię
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Nazwisko
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Nazwa firmy (opcjonalna)
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Adres email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Id adresu w tabeli Address
        /// </summary>
        public int? AdrressId { get; set; }

        /// <summary>
        /// Adres
        /// </summary>
        public virtual Address Address { get; set; }
    }
}
