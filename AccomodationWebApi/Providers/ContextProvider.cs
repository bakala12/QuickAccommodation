using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccommodationDataAccess.Domain;

namespace AccomodationWebApi.Providers
{
    /// <summary>
    /// Represents an object that provides an access to database context.
    /// </summary>
    public class ContextProvider<T> : IContextProvider
        where T : IAccommodationContext, new()
    {
        /// <summary>
        /// Gets a IAccommodationContext which controller can use for work with database.
        /// </summary>
        /// <returns>IAccommodationContext instance.</returns>
        public IAccommodationContext GetNewContext()
        {
            return new T();
        }
    }
}