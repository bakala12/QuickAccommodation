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
using AccommodationApplication.Services;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// View model odpowiedzialny za wyświetlenie wyniku wyszukiwania oferty
    /// </summary>
    public class DisplayableOfferViewModel : ViewModelBase
    {
        private readonly OffersProxy _service = new OffersProxy();
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
            if (off == null) return;
            int id = off.Id;
            string username = Thread.CurrentPrincipal.Identity.Name;
            try
            {
                await _service.ReserveOffer(username, id);
                OfferReserved?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Oferta została zarezerwowana");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Oferta już zarezerwowana.", "Błąd");
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd systemu rezerwacji");
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
        /// Asynchronicznie rezygnuje z oferty
        /// </summary>
        /// <param name="x">Parametr</param>
        /// <returns></returns>
        public async Task ResignAsync(object x)
        {
            DisplayableOffer offer = x as DisplayableOffer;
            if (offer == null) return;
            string username = Thread.CurrentPrincipal.Identity.Name;
            try
            {
                await _service.ResignOffer(username, offer.Id);
                OfferResigned?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Rezygnacja z oferty została dokonana pomyślnie");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Oferta nie była zarezerwowana.", "Błąd");
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd systemu rezerwacji");
            }
        }

        /// <summary>
        /// Wydarzenie podnoszone po zarezerwowaniu oferty.
        /// </summary>
        public event EventHandler OfferReserved;
        /// <summary>
        /// Wydarzenie podnoszone po rezygnacji z oferty.
        /// </summary>
        public event EventHandler OfferResigned;
    }
}
