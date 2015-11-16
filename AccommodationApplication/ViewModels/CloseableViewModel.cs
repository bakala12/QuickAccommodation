using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// ViewModel odpowiadający za zamykające się ViewModele (na przykład dla okien dialogowych)
    /// </summary>
    public abstract class CloseableViewModel : ViewModelBase
    {
        /// <summary>
        /// Zdarzenie wznoszone gdy praca została zakończona i ViewModel może zostac zamknięty
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Metoda wznosząca zdarzenie RequestClose
        /// </summary>
        protected virtual void Close()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
