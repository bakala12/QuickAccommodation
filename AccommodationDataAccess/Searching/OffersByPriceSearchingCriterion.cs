using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public class OffersByPriceSearchingCriterion : OfferSearchingCriterion
    {
        public double? MinimalPrice { get; set; }
        public double? MaximalPrice { get; set; }

        public OffersByPriceSearchingCriterion() : base(SearchingCriterionType.Price)
        {
        }

        public OffersByPriceSearchingCriterion(double? minimalPrice, double? maximalPrice) : this()
        {
            MinimalPrice = minimalPrice;
            MaximalPrice = maximalPrice;
        }

        public override bool IsGood(AvailableOffer parameter)
        {
            if (parameter?.OfferInfo == null || parameter.OfferInfo.Price < 0) throw new InvalidOperationException("Niepełna lub nieprawidłowa informacja o cenie");
            bool b = true;
            if (MinimalPrice.HasValue)
                b = parameter.OfferInfo.Price >= MinimalPrice.Value;
            if (MaximalPrice.HasValue)
                b = b && parameter.OfferInfo.Price <= MaximalPrice.Value;
            return b;
        }
    }
}
