using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication.Interfaces
{

    /// <summary>
    /// Interfejs do identyfikacji widoków w głównym oknie
    /// </summary>
    public interface IPageViewModel
    {
        /// <summary>
        /// Nazwa widoku
        /// </summary>
        string Name { get; }
    }
}
