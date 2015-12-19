using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AccommodationApplication.Model;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    /// <summary>
    /// ViewModel odpowiadający za wyszukiwanie ofert po cenie
    /// </summary>
    public class PriceSearchingViewModel : SearchingViewModelBase
    {
        private double? _minimalPrice;
        private double? _maximalPrice;

        /// <summary>
        /// Pobiera lub ustawia cenę minimalną
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
        /// Pobiera lub ustawia cenę maksymalną
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
        /// Odpowiednie kryetrium wyszukiwania
        /// </summary>
        public override ISearchingCriterion<Offer> Criterion =>
            OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(MinimalPrice, MaximalPrice);

        /// <summary>
        /// Znajduje wyniki wyszukiwania dla ceny.
        /// </summary>
        /// <returns></returns>
        public override async Task SearchAsync()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            IEnumerable<Offer> offers =
                await Service.SearchByPriceAsync(username, MinimalPrice, MaximalPrice, SelectedSortType, SelectedSortBy);
            SearchingResults = offers.Select(o => new DisplayableOfferViewModel(new DisplayableOffer(o)));
        }
    }
}
