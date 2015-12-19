using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Model;
using AccommodationApplication.Services;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;
using AccommodationShared.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    /// <summary>
    /// Bazowa klasa dla ViewModeli do szukania
    /// </summary>
    public abstract class SearchingViewModelBase : ViewModelBase
    {
        protected SearchProxy Service { get; }
        /// <summary>
        /// Inicjalizuje początkowy stan obiektu
        /// </summary>
        protected SearchingViewModelBase()
        {
            Service=new SearchProxy();
            SearchCommand = new DelegateCommand(async x=>await SearchAsync());
            (App.Current as App).Login += (x,e)=> { SearchingResults = null; };
        }

        private IEnumerable<DisplayableOfferViewModel> _searchingResults;

        /// <summary>
        /// Pobiera Pobiera lub ustawia kolekcję z wynikami wyszukiwania
        /// </summary>
        public IEnumerable<DisplayableOfferViewModel> SearchingResults
        {
            get { return _searchingResults; }
            set
            {
                _searchingResults = value;
                OnPropertyChanged();
            }
        } 

        /// <summary>
        /// Komenda reagująca na szukanie
        /// </summary>
        public ICommand SearchCommand { get; protected set; }

        /// <summary>
        /// Abstrakcyjna właściwość z kryterium wyszukiwania
        /// </summary>
        public abstract ISearchingCriterion<Offer> Criterion { get; }

        /// <summary>
        /// Kolekcja dostępnych typów wyszukiwania
        /// </summary>
        public IEnumerable<SortType> SortTypes => (IEnumerable<SortType>)Enum.GetValues(typeof (SortType));
        /// <summary>
        /// Kolekcja dostępnych wartości po których można wyszukiwać
        /// </summary>
        public IEnumerable<SortBy> SortByValues => (IEnumerable<SortBy>) Enum.GetValues(typeof (SortBy));

        private SortType _selectedSortType;

        /// <summary>
        /// Pobiera lub ustawia aktualnie wybrany typ sortowania
        /// </summary>
        public SortType SelectedSortType
        {
            get { return _selectedSortType; }
            set
            {
                _selectedSortType = value;
                OnPropertyChanged();
            }
        }

        private SortBy _selectedSortBy;

        /// <summary>
        /// Pobiera lub ustawia aktualną wartość po której wyszukujemy
        /// </summary>
        public SortBy SelectedSortBy
        {
            get { return _selectedSortBy; }
            set
            {
                _selectedSortBy = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Asynchronicznie wyszukuje oferty w oparciu o kryterium wyszukiwania 
        /// </summary>
        /// <returns></returns>
        public abstract Task SearchAsync();
    }
}
