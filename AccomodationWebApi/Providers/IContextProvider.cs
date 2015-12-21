using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;

namespace AccomodationWebApi.Providers
{
    public interface IContextProvider
    {
        IAccommodationContext GetNewContext();
    }
}
