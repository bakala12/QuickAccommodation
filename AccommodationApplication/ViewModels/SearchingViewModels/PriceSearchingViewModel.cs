using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    public class PriceSearchingViewModel : SearchingViewModelBase
    {
        private double? _minimalPrice;
        private double? _maximalPrice;

        public double? MinimalPrice
        {
            get { return _minimalPrice; }
            set
            {
                _minimalPrice = value;
                OnPropertyChanged();
            }
        }

        public double? MaximalPrice
        {
            get { return _maximalPrice; }
            set
            {
                _maximalPrice = value;
                OnPropertyChanged();
            }
        }

        public override ISearchingCriterion<AvailableOffer> Criterion
            => OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(MinimalPrice, MaximalPrice);
    }
}
