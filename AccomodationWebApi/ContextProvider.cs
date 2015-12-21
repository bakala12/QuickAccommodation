using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccommodationDataAccess.Domain;

namespace AccomodationWebApi.Providers
{
    public class ContextProvider<T> : IContextProvider
        where T:IAccommodationContext, new()
    {
        public IAccommodationContext GetNewContext()
        {
            return new T();
        }
    }
}