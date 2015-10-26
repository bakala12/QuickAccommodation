using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class UserData : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        public int AdrressId { get; set; }
        public virtual Address Address { get; set; }

        public int UserId { get; set; }
        [ForeignKey("LoggedUser")]
        public virtual LoggedUser User { get; set; }
    }
}
