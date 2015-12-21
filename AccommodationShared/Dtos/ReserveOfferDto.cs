using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla rezerwacji oferty
    /// </summary>
    public class ReserveOfferDto
    {
        public string Username { get; set; }
        public int OfferId { get; set; }
    }
}
