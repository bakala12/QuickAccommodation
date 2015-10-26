using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication.ViewModels
{
    public abstract class CloseableViewModel : ViewModelBase
    {
        protected CloseableViewModel() { }

        public event EventHandler RequestClose;

        protected virtual void Close()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
