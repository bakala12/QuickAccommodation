using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationDataAccess.Searching;

namespace AccommodationApplication.ViewModels.SearchingViewModels
{
    public class DateSearchingViewModel :SearchingViewModelBase
    {
        private DateTime? _minimalDate;
        private DateTime? _maximalDate;

        public DateTime? MinimalDate
        {
            get { return _minimalDate; }
            set
            {
                _minimalDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? MaximalDate
        {
            get { return _maximalDate; }
            set
            {
                _maximalDate = value;
                OnPropertyChanged();
            }
        }

        public override ISearchingCriterion<AvailableOffer> Criterion 
            => OffersSearchingCriteriaFactory.CreateDateSearchingCriterion(MinimalDate, MaximalDate);
    }
}
