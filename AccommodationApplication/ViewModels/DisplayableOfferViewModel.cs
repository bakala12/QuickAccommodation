using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Model;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace AccommodationApplication.ViewModels
{
    public class DisplayableOfferViewModel : ViewModelBase
    {
        public ICommand ReserveCommand { get; }

        public DisplayableOfferViewModel(DisplayableSearchResult offer)
        {
            ReserveCommand = new DelegateCommand(async o=>await ReserveAsync(o));
            ResignCommand =new DelegateCommand(async o=>await ResignAsync(o));
            Offer = offer;
        }

        public async Task ReserveAsync(object o)
        {
            DisplayableOffer off= o as DisplayableOffer;
            int id = off.Id;
            string username = Thread.CurrentPrincipal.Identity.Name;
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    User u = await context.Users.FirstOrDefaultAsync(us => us.Username.Equals(username));
                    Offer offer = await context.Offers.FirstOrDefaultAsync(of => of.Id == id);
                    if (u == null || offer == null || offer.IsBooked) throw new Exception();
                    offer.IsBooked = true;
                    offer.Customer = u;
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
            }
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

        public ICommand ResignCommand { get; private set; }

        public void Resign(object x)
        {
            DisplayableOffer offer=x as DisplayableOffer;
            if (offer == null) throw new Exception();
            string username = Thread.CurrentPrincipal.Identity.Name;
            if (string.IsNullOrEmpty(username)) throw new Exception();
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    User u = context.Users.FirstOrDefault(us => us.Username.Equals(username));
                    if (u == null) throw new Exception();
                    Offer o = context.Offers.FirstOrDefault(of => of.Id == offer.Id);
                    if (o == null) throw new Exception();
                    o.Customer = null;
                    o.IsBooked = false;
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public async Task ResignAsync(object x)
        {
            await Task.Run(() => Resign(x));
        }
    }
}
