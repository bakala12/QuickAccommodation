using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;
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

        private IEnumerable<Offer> _searchingResults;

        public IEnumerable<Offer> SearchingResults
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
                SearchingResults = context.Offers.Where(Criterion.SelectableExpression);
            }
        } 
    }
}
