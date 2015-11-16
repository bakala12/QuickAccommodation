using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    /// <summary>
    /// ViewModel odpowiadający za wyszukiwanie ofert po miejscu
    /// </summary>
    public class PlaceSearchingViewModel : SearchingViewModelBase
    {
        private string _placeName;
        private string _cityName;

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
        /// Zwraca odpowiednie kryterium wyszukiwania
        /// </summary>
        public override ISearchingCriterion<Offer> Criterion 
            => OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(PlaceName, CityName);
    }
}
