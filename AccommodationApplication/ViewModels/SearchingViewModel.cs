using AccommodationApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels
{
    public class SearchingViewModel : ViewModelBase, IPageViewModel
    {
        public string Name => "Wyszukiwanie";

        public SearchingViewModel()
        {
            SearchCommand = new DelegateCommand(async x => await SearchAsync());
        }

        private string _placeName;
        private ObservableCollection<AvailableOffer> _searchResults;

        public ObservableCollection<AvailableOffer> SearchResults
        {
            get { return _searchResults; }
            private set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        public string PlaceName
        {
            get { return _placeName; }
            set
            {
                _placeName = value;
                OnPropertyChanged();
            }
        }

        public async Task SearchAsync()
        {
            await Task.Run(() => Search());
        }

        private void Search()
        {
            ISearchingCriterion<AvailableOffer> criterion =
                OffersSearchingCriteriaFactory.CreatePlaceSearchingCriterion(PlaceName);
            using (var context = new AccommodationContext())
            {
                var col = context.AvailableOffers.Where(criterion.SelectableExpression);
                SearchResults = new ObservableCollection<AvailableOffer>(col);
            }
        }
    }
}
