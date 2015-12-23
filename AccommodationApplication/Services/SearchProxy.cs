using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;
using AccommodationShared.Dtos;
using AccommodationShared.Searching;

namespace AccommodationApplication.Services
{

    /// <summary>
    /// Proxy dla wyszukiwania
    /// </summary>
    public class SearchProxy : WebApiProxy
    {
        public SearchProxy() : base("Search")
        {
            
        }

        public async Task<IEnumerable<Offer>> SearchByPlaceAsync(string username, string placeName, string cityName,
            SortType sortType, SortBy sortBy)
        {
            PlaceSearchRequestDto dto = new PlaceSearchRequestDto()
            {
                Username = username,
                PlaceName = placeName,
                CityName = cityName,
                SortType = sortType,
                SortBy = sortBy
            };
            var res= await Post<PlaceSearchRequestDto, SearchResultDto>("place", dto);
            return res.Offers;
        }

        public async Task<IEnumerable<Offer>> SearchByDateAsync(string username, DateTime? minimalDate, DateTime? maximalDate,
            bool showPartiallyMatchingResults, SortType sortType, SortBy sortBy)
        {
            DateSearchRequestDto dto = new DateSearchRequestDto()
            {
                Username = username,
                MinimalDate = minimalDate,
                MaximalDate = maximalDate,
                ShowPartiallyMatchingResults = showPartiallyMatchingResults,
                SortType = sortType,
                SortBy = sortBy
            };
            var res= await Post<DateSearchRequestDto, SearchResultDto>("date", dto);
            return res.Offers;
        }

        public async Task<IEnumerable<Offer>> SearchByPriceAsync(string username, double? minimalPrice, double? maximalPrice,
            SortType sortType, SortBy sortBy)
        {
            PriceSearchRequestDto dto = new PriceSearchRequestDto()
            {
                Username = username,
                MinimalPrice = minimalPrice,
                MaximalPrice = maximalPrice,
                SortBy = sortBy,
                SortType = sortType
            };
            var res= await Post<PriceSearchRequestDto, SearchResultDto>("price", dto);
            return res.Offers;
        }

        public async Task<IEnumerable<Offer>> SearchByMultipleCriteria(string username, string placeName, string cityName,
            DateTime? minimalDate, DateTime? maximalDate, double? minimalPrice, double? maximalPrice, SortType sortType, SortBy sortBy)
        {
            AdvancedSearchRequestDto dto = new AdvancedSearchRequestDto()
            {
                Username = username,
                PlaceName = placeName,
                CityName = cityName,
                MinimalDate = minimalDate,
                MaximalDate = maximalDate,
                MinimalPrice = minimalPrice,
                MaximalPrice = maximalPrice,
                SortBy = sortBy,
                SortType = sortType
            };
            var res = await Post<AdvancedSearchRequestDto, SearchResultDto>("advanced", dto);
            return res.Offers;
        }
    }
}
