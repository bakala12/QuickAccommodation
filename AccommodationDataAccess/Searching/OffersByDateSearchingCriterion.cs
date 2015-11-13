using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public class OffersByDateSearchingCriterion : OfferSearchingCriterion
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

        public override bool IsGood(AvailableOffer parameter)
        {
            if(parameter?.OfferInfo==null) throw new InvalidOperationException("Niepełna informacja o ofercie");
            bool min = !MinimalDate.HasValue || MinimalDate.Value <= parameter.OfferInfo.OfferStartTime;
            bool max = !MaximalDate.HasValue || MaximalDate.Value >= parameter.OfferInfo.OfferEndTime;
            return ShowPartiallyMatchingResults ? min || max : min && max;
        }
    }
}
