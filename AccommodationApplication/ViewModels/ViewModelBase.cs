using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AccommodationApplication.Annotations;
using MahApps.Metro.Controls.Dialogs;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// Bazowa klasa wszystkich ViewModeli. Zapewnia implementację interfejsu INotifyPropertyChanged.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Zdarzenie PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Wznosi zdarzenie PropertyChanged dla zadanej właściwości. Bez podania prametrów, dzięki 
        /// atrybutowi caller member name nie trzeba podawać parametrów.
        /// </summary>
        /// <param name="propertyName">Nazwa zmienionej właściwości</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
