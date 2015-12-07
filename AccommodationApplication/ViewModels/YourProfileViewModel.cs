using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationApplication.Interfaces;

namespace AccommodationApplication.ViewModels
{
    public class YourProfileViewModel : ViewModelBase, IPageViewModel
    {
        public string Name => "Twój profil";
    }
}
