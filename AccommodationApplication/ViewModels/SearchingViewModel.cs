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
using AccommodationApplication.ViewModels.SearchingViewModels;
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
            PlaceSearchingViewModel = new PlaceSearchingViewModel();
            DateSearchingViewModel = new DateSearchingViewModel();
            PriceSearchingViewModel = new PriceSearchingViewModel();
        }

        public PlaceSearchingViewModel PlaceSearchingViewModel { get; }
        public DateSearchingViewModel DateSearchingViewModel { get; }
        public PriceSearchingViewModel PriceSearchingViewModel { get; }
    }
}
