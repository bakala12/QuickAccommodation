using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationShared.Searching;

namespace AccommodationShared.Dtos
{
    public class SearchRequestDto
    {
        public string Username { get; set; }
        public SortType SortType { get; set; }
        public SortBy SortBy { get; set; }
    }

    public class PlaceSearchRequestDto : SearchRequestDto
    {
        public string PlaceName { get; set; }
        public string CityName { get; set; }
    }

    public class DateSearchRequestDto : SearchRequestDto
    {
        public DateTime? MinimalDate { get; set; }
        public DateTime? MaximalDate { get; set; }
        public bool ShowPartiallyMatchingResults { get; set; }
    }

    public class PriceSearchRequestDto : SearchRequestDto
    {
        public double? MinimalPrice { get; set; }
        public double? MaximalPrice { get; set; }
    }
}
