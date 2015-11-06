using AccommodationApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication.ViewModels
{
    public class OffersViewModel : IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Offers";
            }
        }
    }
}
