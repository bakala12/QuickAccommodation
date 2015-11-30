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
    /// <summary>
    /// View model odpowiedzialny za wyświetlenie wyniku wyszukiwania oferty
    /// </summary>
    public class DisplayableOfferViewModel : ViewModelBase
    {
        /// <summary>
        /// Komenda rezerwacji oferty
        /// </summary>
        public ICommand ReserveCommand { get; }

        /// <summary>
        /// Inicjalizuje nową instancję klasy DisplayableOfferViewModel
        /// </summary>
        /// <param name="offer">Oferta stowarzyszona z bieżącą instancją</param>
        public DisplayableOfferViewModel(DisplayableOffer offer)
        {
            ReserveCommand = new DelegateCommand(async o=>await ReserveAsync(o));
            ResignCommand =new DelegateCommand(async o=>await ResignAsync(o));
            Offer = offer;
        }

        /// <summary>
        /// Asynchronicznie dokonuje rezerwacji oferty
        /// </summary>
        /// <param name="o">Parametr komendy</param>
        /// <returns></returns>
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

        private DisplayableOffer _offer;

        /// <summary>
        /// Pobiera lub ustawia ofertę stowarzyszoną z bieżącą instancją ViewModelu
        /// </summary>
        public DisplayableOffer Offer
        {
            get { return _offer; }
            set
            {
                _offer = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Komenda rezygnacji z oferty
        /// </summary>
        public ICommand ResignCommand { get; private set; }

        /// <summary>
        /// Rezygnuje zi bieżącej oferty
        /// </summary>
        /// <param name="x">Parametr</param>
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

        /// <summary>
        /// Asynchronicznie rezygnuje z oferty
        /// </summary>
        /// <param name="x">Parametr</param>
        /// <returns></returns>
        public async Task ResignAsync(object x)
        {
            await Task.Run(() => Resign(x));
        }
    }
}
