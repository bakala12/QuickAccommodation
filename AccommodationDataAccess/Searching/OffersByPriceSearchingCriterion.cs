using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    /// <summary>
    /// Klasa oferująca implementacjeterium wyszukiwania ofert po cenie
    /// </summary>
    internal class OffersByPriceSearchingCriterion : OfferSearchingCriterion
    {
        /// <summary>
        /// Pobiera lub ustawia cenę minimalną do wyszukiwania
        /// </summary>
        public double? MinimalPrice { get; set; }
        /// <summary>
        /// Pobiera lub ustawia cenę maksymalną do wyszukiwania
        /// </summary>
        public double? MaximalPrice { get; set; }

        /// <summary>
        /// Inicjalizuje nowy obiekt kryterium wyszukiwania nie ustawiając żadnych właściwości
        /// </summary>
        public OffersByPriceSearchingCriterion() : base(SearchingCriterionType.Price)
        {
        }

        /// <summary>
        /// Inicjalizuje nowy obiekt wyszukiwania z ustalonymi wartościami ceny minimalnej i maksymalnej
        /// </summary>
        /// <param name="minimalPrice">Cena minimalna</param>
        /// <param name="maximalPrice">Cena maksymalna</param>
        public OffersByPriceSearchingCriterion(double? minimalPrice, double? maximalPrice) : this()
        {
            MinimalPrice = minimalPrice;
            MaximalPrice = maximalPrice;
        }

        /// <summary>
        /// Wyrażenie podawane do LINQ realizujące odpowiednie wyszukiwanie.
        /// </summary>
        public override Expression<Func<Offer, bool>> SelectableExpression
        {
            get
            {
                return parameter =>
                    (!MinimalPrice.HasValue || parameter.OfferInfo.Price >= MinimalPrice.Value) &&
                    (!MaximalPrice.HasValue || parameter.OfferInfo.Price <= MaximalPrice.Value);
            }
        }
    }
}
