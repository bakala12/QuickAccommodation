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
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    /// <summary>
    /// Bazowa klasa dla ViewModeli do szukania
    /// </summary>
    public abstract class SearchingViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Inicjalizuje początkowy stan obiektu
        /// </summary>
        protected SearchingViewModelBase()
        {
            SearchCommand = new DelegateCommand(async x=>await SearchAsync<AccommodationContext>());
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
        public async Task SearchAsync<T>() where T:IAccommodationContext, IDisposable, new()
        {
            await Task.Run(() => Search<T>());
        }

        /// <summary>
        /// Wyszukuje oferty w oparciu o kryterium wyszukiwania 
        /// </summary>
        protected virtual void Search<T>() where T : IAccommodationContext, IDisposable, new()
        {
            using (var context=new T())
            {
                string userName = Thread.CurrentPrincipal.Identity.Name;
                if(string.IsNullOrEmpty(userName)) throw new Exception();
                User u = context.Users.FirstOrDefault(us => us.Username.Equals(userName));
                if(u==null) throw new Exception();
                IQueryable<Offer> offers = context.Offers.Where(o => o.VendorId != u.Id).Where(o => !o.IsBooked);
                offers=offers.Where(Criterion.SelectableExpression).Include(o=>o.OfferInfo).Include(o=>o.Place.Address);
                IEnumerable<Offer> of = offers.Take(20).OrderBy(SelectedSortType, SelectedSortBy);
                SearchingResults = of.Select(offer => new DisplayableOfferViewModel(new DisplayableSearchResult(offer))).ToList();
            }
        } 
    }

    /// <summary>
    /// Typ sortownia 
    /// </summary>
    public enum SortType
    {
        Ascending,
        Descending,
        NotSort
    }

    /// <summary>
    /// Właściwość po której wyszukujemy
    /// </summary>
    public enum SortBy
    {
        Place,
        City,
        Price,
        StartDate,
        EndDate,
        VacanciesNumber,
        PublishDate
    }

    internal static class Sorter
    {
        public static IQueryable<Offer> OrderBy(this IQueryable<Offer> o, SortType type, SortBy by)
        {
            if (type == SortType.Ascending) return o.OrderBy(by);
            if (type == SortType.Descending) return o.OrderByDescending(by);
            return o;
        }

        private static IQueryable<Offer> OrderBy(this IQueryable<Offer> o, SortBy by)
        {
            switch (by)
            {
                case SortBy.Place:
                    return o.OrderBy(x => x.Place.PlaceName);
                case SortBy.City:
                    return o.OrderBy(x => x.Place.Address.City);
                case SortBy.Price:
                    return o.OrderBy(x => x.OfferInfo.Price);
                case SortBy.StartDate:
                    return o.OrderBy(x => x.OfferInfo.OfferStartTime);
                case SortBy.EndDate:
                    return o.OrderBy(x => x.OfferInfo.OfferEndTime);
                case SortBy.VacanciesNumber:
                    return o.OrderBy(x => x.OfferInfo.AvailableVacanciesNumber);
                case SortBy.PublishDate:
                    return o.OrderBy(x => x.OfferInfo.OfferPublishTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(@by), @by, null);
            }
        }

        private static IQueryable<Offer> OrderByDescending(this IQueryable<Offer> o, SortBy by)
        {
            switch (by)
            {
                case SortBy.Place:
                    return o.OrderByDescending(x => x.Place.PlaceName);
                case SortBy.City:
                    return o.OrderByDescending(x => x.Place.Address.City);
                case SortBy.Price:
                    return o.OrderByDescending(x => x.OfferInfo.Price);
                case SortBy.StartDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferStartTime);
                case SortBy.EndDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferEndTime);
                case SortBy.VacanciesNumber:
                    return o.OrderByDescending(x => x.OfferInfo.AvailableVacanciesNumber);
                case SortBy.PublishDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferPublishTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(@by), @by, null);
            }
        }
    }
}
