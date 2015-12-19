using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    public class OfferAllDataDto
    {
        public OfferInfo OfferInfo { get; set; }
        public Place Place { get; set; }
        public User Vendor { get; set; }
    }
}
