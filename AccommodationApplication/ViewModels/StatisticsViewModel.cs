using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccommodationApplication.Interfaces;

namespace AccommodationApplication.ViewModels
{
    public class StatisticsViewModel : ViewModelBase,IPageViewModel
    {
        private string _rankName;
        private int _myOffersCount;
        private int _reservedOffersCount;

        public string Name => "Statystyki";

        public string LoggedUser => Thread.CurrentPrincipal?.Identity?.Name;

        public string RankName
        {
            get { return _rankName; }
            set
            {
                _rankName = value;
                OnPropertyChanged();
            }
        }

        public int MyOffersCount
        {
            get { return _myOffersCount; }
            set
            {
                _myOffersCount = value;
                OnPropertyChanged();
            }
        }

        public int ReservedOffersCount
        {
            get { return _reservedOffersCount; }
            set
            {
                _reservedOffersCount = value;
                OnPropertyChanged();
            }
        }
    }
} 
