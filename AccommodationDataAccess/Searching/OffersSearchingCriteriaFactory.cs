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
        public static ISearchingCriterion<AvailableOffer> CreatePlaceSearchingCriterion(string placeName,
            string city = null)
        {
            return new OffersByPlaceSearchingCriterion(city, placeName);
        }

        public static ISearchingCriterion<AvailableOffer> CreateDateSearchingCriterion(DateTime? minimalDate,
            DateTime? maximalDate)
        {
            return new OffersByDateSearchingCriterion(minimalDate, maximalDate);
        }

        public static ISearchingCriterion<AvailableOffer> CreatePriceSearchingCriterion(double? minimalPrice,
            double? maximalPrice)
        {
            return new OffersByPriceSearchingCriterion(minimalPrice, maximalPrice);
        } 
    }
}
