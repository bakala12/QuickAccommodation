using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla edycji oferty
    /// </summary>
    public class OfferEditDataDto : OfferAllDataDto
    {
        public string Username { get; set; }
        public int OfferId { get; set; }
    }
}
