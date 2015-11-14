using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public class OffersSearchingCriteriaFactory
    {
        public static ISearchingCriterion<Offer> CreatePlaceSearchingCriterion(string placeName,
            string city = null)
        {
            return new OffersByPlaceSearchingCriterion(city, placeName);
        }

        public static ISearchingCriterion<Offer> CreateDateSearchingCriterion(DateTime? minimalDate,
            DateTime? maximalDate, bool showPartiallyMatchingResults = false)
        {
            return new OffersByDateSearchingCriterion(minimalDate, maximalDate, showPartiallyMatchingResults);
        }

        public static ISearchingCriterion<Offer> CreatePriceSearchingCriterion(double? minimalPrice,
            double? maximalPrice)
        {
            return new OffersByPriceSearchingCriterion(minimalPrice, maximalPrice);
        } 
    }
}
