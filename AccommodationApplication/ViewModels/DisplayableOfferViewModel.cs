using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Model;

namespace AccommodationApplication.ViewModels
{
    public class DisplayableOfferViewModel : ViewModelBase
    {
        public ICommand ReserveCommand { get; }

        public DisplayableOfferViewModel(DisplayableSearchResult offer)
        {
            ReserveCommand = new DelegateCommand(x=>Reserve(x));
            Offer = offer;
        }

        private void Reserve(object o)
        {
            MessageBox.Show("ha");
        }

        private DisplayableSearchResult _offer;

        public DisplayableSearchResult Offer
        {
            get { return _offer; }
            set
            {
                _offer = value;
                OnPropertyChanged();
            }
        }
    }
}
