using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public interface IOfferSearcher
    {
        IEnumerable<ISearchingCriterion<AvailableOffer>> SearchingCriteria { get; }
        IEnumerable<AvailableOffer> FindMatchingOffers<T>() where T : IOffersContext, IDisposable, new();
        Task<IEnumerable<AvailableOffer>> FindMatchingOffersAsync<T>() where T : IOffersContext, IDisposable, new();
        ResultSortType ResultSortType { get; }
    }
}
