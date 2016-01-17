using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;
using AccommodationShared.Searching;
using AccommodationWebPage.Models;

namespace AccommodationWebPage.DataAccess
{
    /// <summary>
    /// Klasa do wyszukiwania ofert.
    /// </summary>
    public class SearchDataAccessor
    {
        /// <summary>
        /// Wyszukuje oferty.
        /// </summary>
        /// <param name="context">Kontkst bazy danych</param>
        /// <param name="username">Nazwa użytkwnika</param>
        /// <param name="criteria">Kryteria wyszukiwania</param>
        /// <param name="sortBy">Po jakiej właściwości posortować wyniki</param>
        /// <param name="sortType">Porządek sortowania.</param>
        /// <returns>Lista z wynikami wyszukiwania.</returns>
        private IList<OfferViewModel> Search(IAccommodationContext context, string username, 
            ISearchingCriterion<Offer>[] criteria, SortType sortType, SortBy sortBy)
        {
            if (string.IsNullOrEmpty(username)) return null;
            User u = context.Users.FirstOrDefault(us => us.Username.Equals(username));
            if (u == null) return null;
            IQueryable<Offer> offers = context.Offers.Where(o => o.VendorId != u.Id).Where(o => !o.IsBooked);
            offers = criteria.Aggregate(offers, (current, criterion) => current.Where(criterion.SelectableExpression));
            offers = offers.Take(20).OrderBy(sortType, sortBy);
            return offers.ToList().Select(offer => new OfferViewModel(offer)).ToList();
        }

        public IList<OfferViewModel> SearchByPlace(IAccommodationContext context, PlaceSearchingModel model)
        {
            ISearchingCriterion<Offer> criterion =
                OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(model.PlaceName, model.CityName);
            return Search(context,model.Username, new[] { criterion }, model.SortType, model.SortBy);
        }

        public IList<OfferViewModel> SearchByDate(IAccommodationContext context,DateSearchingModel model)
        {
            ISearchingCriterion<Offer> criterion =
                OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(model.MinimalDate, model.MaximalDate,
                    model.ShowPartiallyMatchingResults);
            return Search(context,model.Username, new[] { criterion }, model.SortType, model.SortBy);
        }

        public IList<OfferViewModel> SearchByPrice(IAccommodationContext context,PriceSearchingModel model)
        {
            ISearchingCriterion<Offer> criterion =
                OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(model.MinimalPrice, model.MaximalPrice);
            return Search(context,model.Username, new[] { criterion }, model.SortType, model.SortBy);
        }

        public IList<OfferViewModel> SearchByMultipleCriteria(IAccommodationContext context,AdvancedSearchingModel model)
        {
            ISearchingCriterion<Offer>[] criteria = new[]
            {
                OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(model.PlaceName, model.CityName),
                OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(model.MinimalDate, model.MaximalDate),
                OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(model.MinimalPrice, model.MaximalPrice)
            };
            return Search(context,model.Username, criteria, model.SortType, model.SortBy);
        }

        public async Task<IList<OfferViewModel>> SearchByPlaceAsync(IAccommodationContext context, PlaceSearchingModel model)
        {
            return await Task.Run(() => SearchByPlace(context, model));
        }

        public async Task<IList<OfferViewModel>> SearchByDateAsync(IAccommodationContext context, DateSearchingModel model)
        {
            return await Task.Run(() => SearchByDate(context, model));
        }

        public async Task<IList<OfferViewModel>> SearchByPriceAsync(IAccommodationContext context, PriceSearchingModel model)
        {
            return await Task.Run(() => SearchByPrice(context, model));
        }

        public async Task<IList<OfferViewModel>> SearchByMultipleCriteriaAsync(IAccommodationContext context,
            AdvancedSearchingModel model)
        {
            return await Task.Run(() => SearchByMultipleCriteria(context, model));
        }
    }
}