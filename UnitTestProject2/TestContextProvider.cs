using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Providers;

namespace UnitTestProject2
{
    public class TestContextProvider : IContextProvider
    {
        private readonly AccommodationMockContext _context ;
        public TestContextProvider(AccommodationMockContext context)
        {
            if(context==null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }
        public IAccommodationContext GetNewContext()
        {
            return _context;
        }
    }
}
