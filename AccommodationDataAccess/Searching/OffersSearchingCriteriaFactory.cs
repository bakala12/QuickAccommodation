using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    /// <summary>
    /// Statyczna klasa oferująca dostęp do kryteriów wyszukiwnia. Realizuje wzorzec projektowy fabryki
    /// </summary>
    public static class OffersSearchingCriteriaFactory
    {
        /// <summary>
        /// Zwraca kryterium wyszukiwania po miejscu
        /// </summary>
        /// <param name="placeName">Nazwa miejsca</param>
        /// <param name="city">Nazwa miasta</param>
        /// <returns>Odpowiednie kryterium wyszukiwania (jako interfejs)</returns>
        public static ISearchingCriterion<Offer> CreatePlaceSearchingCriterion(string placeName,
            string city = null)
        {
            return new OffersByPlaceSearchingCriterion(city, placeName);
        }

        /// <summary>
        /// Zwraca kryterium wyszukiwania po dacie
        /// </summary>
        /// <param name="minimalDate">Data minimalna</param>
        /// <param name="maximalDate">Data maksymalna</param>
        /// <param name="showPartiallyMatchingResults">Informuje czy wyświetlać wyniki częściowo pasujące</param>
        /// <returns>Odpowiednie kryterium wyszukiwania (jako interfejs)</returns>
        public static ISearchingCriterion<Offer> CreateDateSearchingCriterion(DateTime? minimalDate,
            DateTime? maximalDate, bool showPartiallyMatchingResults = false)
        {
            return new OffersByDateSearchingCriterion(minimalDate, maximalDate, showPartiallyMatchingResults);
        }

        /// <summary>
        /// Zwraca kryterium wyszukiwania po cenie
        /// </summary>
        /// <param name="minimalPrice">Cena minimalna</param>
        /// <param name="maximalPrice">Cena maksymalna</param>
        /// <returns>Odpowiednie kryterium wyszukiwania (jako interfejs)</returns>
        public static ISearchingCriterion<Offer> CreatePriceSearchingCriterion(double? minimalPrice,
            double? maximalPrice)
        {
            return new OffersByPriceSearchingCriterion(minimalPrice, maximalPrice);
        } 
    }
}
