using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    internal class OffersByDateSearchingCriterion : OfferSearchingCriterion
    {
        public DateTime? MinimalDate { get; set; }
        public DateTime? MaximalDate { get; set; }
        public bool ShowPartiallyMatchingResults { get; set; }

        public OffersByDateSearchingCriterion() : base(SearchingCriterionType.Date)
        {
            ShowPartiallyMatchingResults = false;
        }

        public OffersByDateSearchingCriterion(DateTime? minimalDate, DateTime? maximalDate) : this()
        {
            MinimalDate = minimalDate;
            MaximalDate = maximalDate;
        }

        public OffersByDateSearchingCriterion(DateTime? minimalDate, DateTime? maximalDate,
            bool showPartiallyMatchingResults) : this(minimalDate, maximalDate)
        {
            ShowPartiallyMatchingResults = showPartiallyMatchingResults;
        }

        public override Expression<Func<AvailableOffer, bool>> SelectableExpression
        {
            get
            {
                return parameter => 
                ShowPartiallyMatchingResults ? !MinimalDate.HasValue || MinimalDate.Value <= parameter.OfferInfo.OfferStartTime
                || !MaximalDate.HasValue || MaximalDate.Value >= parameter.OfferInfo.OfferEndTime 
                : !MinimalDate.HasValue || MinimalDate.Value <= parameter.OfferInfo.OfferStartTime
                && !MaximalDate.HasValue || MaximalDate.Value >= parameter.OfferInfo.OfferEndTime;
            } 
        }
    }
}
