using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla wyników wyszukiwania
    /// </summary>
    public class SearchResultDto
    {
        public IEnumerable<Offer> Offers { get; set; }
    }
}
