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
    public abstract class SearchingViewModelBase : ViewModelBase
    {
        protected SearchingViewModelBase()
        {
            SearchCommand = new DelegateCommand(async x=>await SearchAsync());
            ReserveCommand = new DelegateCommand(x => { MessageBox.Show("ha"); });
        }

        private IEnumerable<DisplayableSearchResult> _searchingResults;

        public IEnumerable<DisplayableSearchResult> SearchingResults
        {
            get { return _searchingResults; }
            set
            {
                _searchingResults = value;
                OnPropertyChanged();
            }
        } 

        public ICommand SearchCommand { get; protected set; }
        public ICommand ReserveCommand { get; protected set; }

        public abstract ISearchingCriterion<Offer> Criterion { get; }

        public IEnumerable<SortType> SortTypes => (IEnumerable<SortType>)Enum.GetValues(typeof (SortType));
        public IEnumerable<SortBy> SortByValues => (IEnumerable<SortBy>) Enum.GetValues(typeof (SortBy));

        private SortType _selectedSortType;

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

        public SortBy SelectedSortBy
        {
            get { return _selectedSortBy; }
            set
            {
                _selectedSortBy = value;
                OnPropertyChanged();
            }
        }

        public async Task SearchAsync()
        {
            await Task.Run(() => Search());
        }

        protected virtual void Search()
        {
            using (var context=new AccommodationContext())
            {
                string userName = Thread.CurrentPrincipal.Identity.Name;
                if(string.IsNullOrEmpty(userName)) throw new Exception();
                User u = context.Users.FirstOrDefault(us => us.Username.Equals(userName));
                if(u==null) throw new Exception();
                IQueryable<Offer> offers = context.Offers.Where(o => o.VendorId != u.Id).Where(o => !o.IsBooked);
                offers=offers.Where(Criterion.SelectableExpression).Include(o=>o.OfferInfo).Include(o=>o.Place.Address);
                IEnumerable<Offer> of = offers.Take(20).OrderBy(SelectedSortType, SelectedSortBy);
                SearchingResults = of.Select(offer => new DisplayableSearchResult(offer)).ToList();
            }
        } 
    }

    public enum SortType
    {
        Ascending,
        Descending,
        NotSort
    }

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
