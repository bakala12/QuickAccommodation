using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public class OffersSearcher : IOfferSearcher
    {
        public OffersSearcher()
        {
            SearchingCriteria = null;
            ResultSortType =ResultSortType.Ascending;
        }

        public OffersSearcher(IEnumerable<ISearchingCriterion<AvailableOffer>> criteria)
        {
            SearchingCriteria = criteria;
        }

        public OffersSearcher(IEnumerable<ISearchingCriterion<AvailableOffer>> criteria, ResultSortType sortType) :this(criteria)
        {
            ResultSortType = sortType;
        }

        public IEnumerable<ISearchingCriterion<AvailableOffer>> SearchingCriteria { get; }

        public IEnumerable<AvailableOffer> FindMatchingOffers<T>() where T : IOffersContext, IDisposable, new()
        {
            using (var context = new T())
            {
                return context.AvailableOffers.Where(o => ResolveAllCriteria(o));
            }
        }

        public async Task<IEnumerable<AvailableOffer>> FindMatchingOffersAsync<T>() where T : IOffersContext, IDisposable, new()
        {
            return await Task.Run(() => FindMatchingOffers<T>());
        }

        public ResultSortType ResultSortType { get; set; }

        private bool ResolveAllCriteria(AvailableOffer offer)
        {
            return SearchingCriteria.Aggregate(true, (current, searchingCriterion) => current && searchingCriterion.IsGood(offer));
        }
    }
}
