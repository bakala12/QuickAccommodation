using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationShared.Searching;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla zapytania
    /// </summary>
    public class SearchRequestDto
    {
        public string Username { get; set; }
        public SortType SortType { get; set; }
        public SortBy SortBy { get; set; }
    }

    /// <summary>
    /// Dto dla szukania po miejscu
    /// </summary>
    public class PlaceSearchRequestDto : SearchRequestDto
    {
        public string PlaceName { get; set; }
        public string CityName { get; set; }
    }

    /// <summary>
    /// Dto dla szukania po dacie
    /// </summary>
    public class DateSearchRequestDto : SearchRequestDto
    {
        public DateTime? MinimalDate { get; set; }
        public DateTime? MaximalDate { get; set; }
        public bool ShowPartiallyMatchingResults { get; set; }
    }

    /// <summary>
    /// Dto dla szukania po cenie
    /// </summary>
    public class PriceSearchRequestDto : SearchRequestDto
    {
        public double? MinimalPrice { get; set; }
        public double? MaximalPrice { get; set; }
    }

    /// <summary>
    /// Dto dla szukania zaawansowanego
    /// </summary>
    public class AdvancedSearchRequestDto : SearchRequestDto
    {
        public string PlaceName { get; set; }
        public string CityName { get; set; }
        public DateTime? MinimalDate { get; set; }
        public DateTime? MaximalDate { get; set; }
        public double? MinimalPrice { get; set; }
        public double? MaximalPrice { get; set; }
    }
}
