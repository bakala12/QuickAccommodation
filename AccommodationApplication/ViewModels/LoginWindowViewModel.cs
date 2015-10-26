using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Commands;

namespace AccommodationApplication.ViewModels
{
    public class LoginWindowViewModel : CloseableViewModel
    {
        public LoginWindowViewModel()
        {
            CloseCommand = new DelegateCommand(x=>Close());
        }

        public ICommand CloseCommand { get; set; }
    }
}
