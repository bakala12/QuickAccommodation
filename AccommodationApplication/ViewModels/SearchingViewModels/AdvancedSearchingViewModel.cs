using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccommodationApplication.Model;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    public class AdvancedSearchingViewModel : SearchingViewModelBase
    {
        private string _placeName;
        private string _cityName;
        private DateTime? _minimalTime;
        private DateTime? _maximalTime;
        private double? _minimalPrice;
        private double? _maximalPrice;

        public string PlaceName
        {
            get { return _placeName; }
            set
            {
                _placeName = value;
                OnPropertyChanged();
            }
        }

        public string CityName
        {
            get { return _cityName; }
            set
            {
                _cityName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? MinimalDate
        {
            get { return _minimalTime; }
            set
            {
                _minimalTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime? MaximalDate
        {
            get { return _maximalTime; }
            set
            {
                _maximalTime = value;
                OnPropertyChanged();
            }
        }

        public double? MinimalPrice
        {
            get { return _minimalPrice; }
            set
            {
                _minimalPrice = value;
                OnPropertyChanged();
            }
        }

        public double? MaximalPrice
        {
            get { return _maximalPrice; }
            set
            {
                _maximalPrice = value;
                OnPropertyChanged();
            }
        }

        public override ISearchingCriterion<Offer> Criterion => null;

        public IEnumerable<ISearchingCriterion<Offer>> Criteria =>
            new ISearchingCriterion<Offer>[]
            {
                OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(PlaceName, CityName),
                OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(MinimalDate, MaximalDate),
                OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(MinimalPrice, MaximalPrice)
            };

        protected override void Search()
        {
            using (var context = new AccommodationContext())
            {
                string userName = Thread.CurrentPrincipal.Identity.Name;
                if (string.IsNullOrEmpty(userName)) throw new Exception();
                User u = context.Users.FirstOrDefault(us => us.Username.Equals(userName));
                if (u == null) throw new Exception();
                IQueryable<Offer> offers = context.Offers.Where(o => o.VendorId != u.Id).Where(o => !o.IsBooked);
                offers = Criteria.Aggregate(offers, (current, criterion) => current.Where(criterion.SelectableExpression));
                offers=offers.Include(o => o.OfferInfo).Include(o => o.Place.Address);
                IEnumerable<Offer> of = offers.Take(20).OrderBy(SelectedSortType, SelectedSortBy);
                SearchingResults = of.Select(offer => new DisplayableOfferViewModel(new DisplayableSearchResult(offer))).ToList();
            }
        }
    }
}
