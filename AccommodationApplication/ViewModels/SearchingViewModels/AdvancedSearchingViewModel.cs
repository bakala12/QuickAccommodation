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
    /// <summary>
    /// ViewModel odpowiadający za zaawansowane wyszukiwanie ofert w bazie
    /// </summary>
    public class AdvancedSearchingViewModel : SearchingViewModelBase
    {
        private string _placeName;
        private string _cityName;
        private DateTime? _minimalTime;
        private DateTime? _maximalTime;
        private double? _minimalPrice;
        private double? _maximalPrice;

        /// <summary>
        /// Pobiera lub ustawia nazwę miejsca
        /// </summary>
        public string PlaceName
        {
            get { return _placeName; }
            set
            {
                _placeName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia nazwę miasta
        /// </summary>
        public string CityName
        {
            get { return _cityName; }
            set
            {
                _cityName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia minimalną datę
        /// </summary>
        public DateTime? MinimalDate
        {
            get { return _minimalTime; }
            set
            {
                _minimalTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia maksymalną datę
        /// </summary>
        public DateTime? MaximalDate
        {
            get { return _maximalTime; }
            set
            {
                _maximalTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia minimalną cenę
        /// </summary>
        public double? MinimalPrice
        {
            get { return _minimalPrice; }
            set
            {
                _minimalPrice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia maksymalną cenę
        /// </summary>
        public double? MaximalPrice
        {
            get { return _maximalPrice; }
            set
            {
                _maximalPrice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Zwraca odpowiednie kryterium wyszukiwania (tutaj null)
        /// </summary>
        public override ISearchingCriterion<Offer> Criterion => null;

        /// <summary>
        /// Zwraca kolekcje odpowiednich kryteriów wyszukiwania (wyszukiwanie oparte o wiele kryteriów)
        /// </summary>
        public IEnumerable<ISearchingCriterion<Offer>> Criteria =>
            new ISearchingCriterion<Offer>[]
            {
                OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(PlaceName, CityName),
                OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(MinimalDate, MaximalDate),
                OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(MinimalPrice, MaximalPrice)
            };

        /// <summary>
        /// Nadpisuje metodę wyszukiwania dla wielu kryteriów
        /// </summary>
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
