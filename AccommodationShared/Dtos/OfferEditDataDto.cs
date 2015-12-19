using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    public class OfferEditDataDto : OfferAllDataDto
    {
        public string Username { get; set; }
        public int OfferId { get; set; }
    }
}
