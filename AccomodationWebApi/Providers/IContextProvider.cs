using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;

namespace AccomodationWebApi.Providers
{
    /// <summary>
    /// Represents an object that provides an access to database context.
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Gets a IAccommodationContext which controller can use for work with database.
        /// </summary>
        /// <returns>IAccommodationContext instance.</returns>
        IAccommodationContext GetNewContext();
    }
}
