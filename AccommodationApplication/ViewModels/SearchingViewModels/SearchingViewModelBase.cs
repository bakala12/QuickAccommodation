using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private IEnumerable<DisplayableOffer> _searchingResults;

        public IEnumerable<DisplayableOffer> SearchingResults
        {
            get { return _searchingResults; }
            set
            {
                _searchingResults = value;
                OnPropertyChanged();
            }
        } 

        public ICommand SearchCommand { get; protected set; }
        public abstract ISearchingCriterion<Offer> Criterion { get; } 

        public async Task SearchAsync()
        {
            await Task.Run(() => Search());
        }

        protected virtual void Search()
        {
            using (var context=new AccommodationContext())
            {
                IEnumerable<Offer> offers=context.Offers.Where(Criterion.SelectableExpression).Include(o=>o.OfferInfo).Include(o=>o.Place.Address);
                SearchingResults = offers.Select(offer => new DisplayableOffer(offer.OfferInfo, offer.Place.Address)).Take(20).ToList();
            }
        } 
    }
}
