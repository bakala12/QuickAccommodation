using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;
using AccommodationShared.Dtos;
using AccommodationShared.Searching;
using AccomodationWebApi.Providers;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/Search")]
    public class SearchController : ApiController
    {

        private readonly IContextProvider _provider;

        public SearchController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public SearchController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }

        private IHttpActionResult Search(string username, ISearchingCriterion<Offer>[] criteria, SortType sortType, SortBy sortBy)
        {
            using (var context = _provider.GetNewContext())
            {
                if(context is DbContext)
                    (context as DbContext).Configuration.ProxyCreationEnabled = false;
                if (string.IsNullOrEmpty(username)) return NotFound();
                User u = context.Users.FirstOrDefault(us => us.Username.Equals(username));
                if (u == null) return NotFound();
                IQueryable<Offer> offers = context.Offers.Where(o => o.VendorId != u.Id).Where(o => !o.IsBooked);
                offers = criteria.Aggregate(offers, (current, criterion) => current.Where(criterion.SelectableExpression));
                offers = offers.Take(20).OrderBy(sortType, sortBy);
                List<Offer> list = offers.ToList();
                SearchResultDto dto = new SearchResultDto()
                {
                    Offers = list
                };
                return Ok(dto);
            }
        }

        [Route("place"), HttpPost]
        public IHttpActionResult SearchByPlace(PlaceSearchRequestDto dto)
        {
            ISearchingCriterion<Offer> criterion =
                OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(dto.PlaceName, dto.CityName);
            return Search(dto.Username, new [] { criterion}, dto.SortType, dto.SortBy);
        }

        [Route("date"), HttpPost]
        public IHttpActionResult SearchByDate(DateSearchRequestDto dto)
        {
            ISearchingCriterion<Offer> criterion =
                OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(dto.MinimalDate, dto.MaximalDate,
                    dto.ShowPartiallyMatchingResults);
            return Search(dto.Username, new[] { criterion }, dto.SortType, dto.SortBy);
        }

        [Route("price"), HttpPost]
        public IHttpActionResult SearchByPrice(PriceSearchRequestDto dto)
        {
            ISearchingCriterion<Offer> criterion =
                OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(dto.MinimalPrice, dto.MaximalPrice);
            return Search(dto.Username, new[] { criterion }, dto.SortType, dto.SortBy);
        }

        [Route("advanced"), HttpPost]
        public IHttpActionResult SearchByMultipleCriteria(AdvancedSearchRequestDto dto)
        {
            ISearchingCriterion<Offer>[] criteria = new[]
            {
                OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(dto.PlaceName, dto.CityName),
                OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(dto.MinimalDate, dto.MaximalDate),
                OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(dto.MinimalPrice, dto.MaximalPrice)
            };
            return Search(dto.Username, criteria, dto.SortType, dto.SortBy);
        }
    }
}
