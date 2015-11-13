using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    internal class OffersByPriceSearchingCriterion : OfferSearchingCriterion
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

        public override Expression<Func<AvailableOffer, bool>> SelectableExpression
        {
            get
            {
                return parameter =>
                    (!MinimalPrice.HasValue || parameter.OfferInfo.Price >= MinimalPrice.Value) &&
                    (!MaximalPrice.HasValue || parameter.OfferInfo.Price <= MaximalPrice.Value);
            }
        }
    }
}
