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
    public class PlaceSearchingViewModel : SearchingViewModelBase
    {
        private string _placeName;
        private string _cityName;

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

        public override ISearchingCriterion<AvailableOffer> Criterion 
            => OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(PlaceName, CityName);
    }
}
