using AccommodationApplication.Interfaces;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace AccommodationApplication.ViewModels
{
    public class OffersViewModel : IPageViewModel
    {
        public string Name
        {
            get
            {
                return "My offers";
            }
        }

        public ObservableCollection<Offer> currentOffersList = new ObservableCollection<Offer>();

        public ObservableCollection<Offer> CurrentOffersList => currentOffersList;




    }
}
