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
            return await Post<PlaceSearchRequestDto, List<Offer>>("place", dto);
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
            return await Post<DateSearchRequestDto, List<Offer>>("date", dto);
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
            return await Post<PriceSearchRequestDto, List<Offer>>("price", dto);
        }

        //public async Task<IEnumerable<Offer>> SearchByMultipleCriteria(string username)
        //{
            
        //}
    }
}
