using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public abstract class OfferSearchingCriterion : ISearchingCriterion<AvailableOffer>
    {
        protected OfferSearchingCriterion(SearchingCriterionType criterionType, ResultSortType sortType)
        {
            CriterionType = criterionType;
            SortType = sortType;
        }

        protected OfferSearchingCriterion(SearchingCriterionType criterionType)
            : this(criterionType, ResultSortType.Ascending)
        {
        }

        public SearchingCriterionType CriterionType { get; }
        public ResultSortType SortType { get; set; }
        public abstract bool IsGood(AvailableOffer parameter);
    }
}
