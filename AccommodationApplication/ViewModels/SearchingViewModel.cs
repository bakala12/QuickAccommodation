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
    /// <summary>
    /// ViewModel nadzorujący funkcjonalność wyszukiwania ofert w aplikacji
    /// </summary>
    public class SearchingViewModel : ViewModelBase, IPageViewModel
    {
        /// <summary>
        /// Nazwa ViewModelu, zaimplementowane z IPageViewModel
        /// </summary>
        public string Name => "Wyszukiwanie";

        /// <summary>
        /// Inicjalizuje nową instancję klasy SearchingViewModel
        /// </summary>
        public SearchingViewModel()
        {
            PlaceSearchingViewModel = new PlaceSearchingViewModel();
            DateSearchingViewModel = new DateSearchingViewModel();
            PriceSearchingViewModel = new PriceSearchingViewModel();
            AdvancedSearchingViewModel = new AdvancedSearchingViewModel();
        }

        /// <summary>
        /// Pobiera instancję ViewModelu odpowiadającą za wyszukiwanie ofert po miejscu
        /// </summary>
        public PlaceSearchingViewModel PlaceSearchingViewModel { get; }
        /// <summary>
        /// Pobiera instancję ViewModelu odpowiadającą za wyszukiwanie ofert po dacie
        /// </summary>
        public DateSearchingViewModel DateSearchingViewModel { get; }
        /// <summary>
        /// Pobiera instancę ViewModelu odpowiedzialną za wyszukiwanie po cenie
        /// </summary>
        public PriceSearchingViewModel PriceSearchingViewModel { get; }
        /// <summary>
        /// Pobiera lub ustawia instancję ViewModelu odpowiedzialną za zaawansowane wyszukiwnaie ofert.
        /// </summary>
        public AdvancedSearchingViewModel AdvancedSearchingViewModel { get; }
    }
}
