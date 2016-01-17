using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Providers;

namespace AccommodationWebPage.Controllers
{
    /// <summary>
    /// Bazowa klasa dla kontrolerów komunikujących się z bazą danych poprzez kontekst IAccommodationContext.
    /// Klasa implementuje wzorzec IDisposable.
    /// </summary>
    public abstract class AccommodationController : Controller
    {
        private readonly IContextProvider _provider;
        private IAccommodationContext _context;

        /// <summary>
        /// Właściwość udostępniająca kontekst bazy danych.
        /// </summary>
        protected IAccommodationContext Context
        {
            get
            {
                if (_context == null)
                    _context = _provider.GetNewContext();
                return _context;
            }
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy AccommodationController używając podanego obiektu IContextProvider
        /// </summary>
        /// <param name="provider"></param>
        protected AccommodationController(IContextProvider provider)
        {
            if(provider==null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        /// <summary>
        /// Zwalnia zasoby (w tym przypadku zamyka kontekst.
        /// </summary>
        /// <param name="disposing">Informuje czy obiekt czy zwolnić zasoby.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Destruktor
        /// </summary>
        ~AccommodationController()
        {
            Dispose(false);
        }
    }
}