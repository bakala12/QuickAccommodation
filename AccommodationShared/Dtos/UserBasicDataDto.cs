using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla podstawowych informacji o userze
    /// </summary>
    public class UserBasicDataDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
    }
}
