using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using AccommodationApplication.Model;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    /// <summary>
    /// ViewModel odpowiadający za wyszukiwanie ofert po dacie
    /// </summary>
    public class DateSearchingViewModel :SearchingViewModelBase
    {
        private DateTime? _minimalDate;
        private DateTime? _maximalDate;
        private bool _showPartiallyMatchingResults;

        /// <summary>
        /// Pobiera lub ustawia minmalną datę
        /// </summary>
        public DateTime? MinimalDate
        {
            get { return _minimalDate; }
            set
            {
                _minimalDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia maksymalną datę
        /// </summary>
        public DateTime? MaximalDate
        {
            get { return _maximalDate; }
            set
            {
                _maximalDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia wartość odpowiadającą za pokazywanie częśćiowo pasujących wyników
        /// </summary>
        public bool ShowPartiallyMatchingResults
        {
            get { return _showPartiallyMatchingResults; }
            set
            {
                _showPartiallyMatchingResults = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Zwraca odpowiednie kryterium wyszukiwania
        /// </summary>
        public override ISearchingCriterion<Offer> Criterion 
            => OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(MinimalDate, MaximalDate, ShowPartiallyMatchingResults);

        /// <summary>
        /// Znajduje pasujące oferty po dacie.
        /// </summary>
        public override async Task SearchAsync()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            IEnumerable<Offer> offers = await Service.SearchByDateAsync(username, MinimalDate, MaximalDate,
                ShowPartiallyMatchingResults, SelectedSortType, SelectedSortBy);
            SearchingResults = offers.Select(o => new DisplayableOfferViewModel(new DisplayableOffer(o)));
        }
    }
}
