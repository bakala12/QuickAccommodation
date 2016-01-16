using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Providers;

namespace AccommodationWebPage.Controllers
{
    public abstract class AccommodationController : Controller
    {
        private readonly IContextProvider _provider;
        private IAccommodationContext _context;

        protected IAccommodationContext Context
        {
            get
            {
                if (_context == null)
                    _context = _provider.GetNewContext();
                return _context;
            }
        }

        protected AccommodationController(IContextProvider provider)
        {
            if(provider==null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            base.Dispose(disposing);
        }

        ~AccommodationController()
        {
            Dispose(false);
        }
    }
}