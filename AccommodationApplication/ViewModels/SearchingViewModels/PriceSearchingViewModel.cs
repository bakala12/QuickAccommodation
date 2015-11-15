using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    public class PriceSearchingViewModel : SearchingViewModelBase
    {
        private double? _minimalPrice;
        private double? _maximalPrice;
        private readonly Regex _moneyRegex = new Regex(@"^[1-9][0-9]*[.[0-9]{2}]?$");

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

        public override ISearchingCriterion<Offer> Criterion
        {
            get
            {
                return OffersSearchingCriteriaFactory.CreatePriceSearchingCriterion(MinimalPrice,MaximalPrice);
            }
        }
    }
}
