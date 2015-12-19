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

        public override async Task<IEnumerable<Offer>>  SearchAsync()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            return await Service.SearchByMultipleCriteria(username, PlaceName, CityName, MinimalDate, MaximalDate,
                MinimalPrice, MaximalPrice, SelectedSortType, SelectedSortBy);
        }
    }
}
