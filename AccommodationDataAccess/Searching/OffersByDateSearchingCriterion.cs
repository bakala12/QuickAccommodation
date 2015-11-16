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
    /// Implementacja kryterium wyszukiwania oferty po dacie rozpoczęcia i zakończenia
    /// </summary>
    internal class OffersByDateSearchingCriterion : OfferSearchingCriterion
    {
        /// <summary>
        /// Pobiera lub ustawia wartość minimalną daty do wyszukiwania
        /// </summary>
        public DateTime? MinimalDate { get; set; }
        /// <summary>
        /// Pobiera lub ustawia wartość maksymalną daty do wyszukiwania
        /// </summary>
        public DateTime? MaximalDate { get; set; }
        /// <summary>
        /// Pobiera lub ustawia wartość czy brać pod uwagę wyniki częściowo pasujące
        /// </summary>
        public bool ShowPartiallyMatchingResults { get; set; }

        /// <summary>
        /// Tworzy nową instancję kryterium wyszukiwania po dacie
        /// </summary>
        public OffersByDateSearchingCriterion() : base(SearchingCriterionType.Date)
        {
            ShowPartiallyMatchingResults = false;
        }
        /// <summary>
        /// Tworzy nową instancję kryterium wyszukiwania po dacie
        /// </summary>
        /// <param name="minimalDate">Minimalna data</param>
        /// <param name="maximalDate">Maksymalna data</param>
        public OffersByDateSearchingCriterion(DateTime? minimalDate, DateTime? maximalDate) : this()
        {
            MinimalDate = minimalDate;
            MaximalDate = maximalDate;
        }

        /// <summary>
        /// Tworzy nową instancję kryterium wyszukiwania po dacie
        /// </summary>
        /// <param name="minimalDate">Minimalna data</param>
        /// <param name="maximalDate">Maksymalna data</param>
        /// <param name="showPartiallyMatchingResults">Informuje czy w wyszukiwnaiu uwzględniac oferty spełniające tylko częściowo podane warunki</param>
        public OffersByDateSearchingCriterion(DateTime? minimalDate, DateTime? maximalDate,
            bool showPartiallyMatchingResults) : this(minimalDate, maximalDate)
        {
            ShowPartiallyMatchingResults = showPartiallyMatchingResults;
        }

        /// <summary>
        /// Wyrażenie podawane do LINQ realizujące odpowiednie wyszukiwanie.
        /// </summary>
        public override Expression<Func<Offer, bool>> SelectableExpression
        {
            get
            {
                return parameter => 
                ShowPartiallyMatchingResults ? !MinimalDate.HasValue || MinimalDate.Value <= parameter.OfferInfo.OfferStartTime
                || !MaximalDate.HasValue || MaximalDate.Value >= parameter.OfferInfo.OfferEndTime 
                : !MinimalDate.HasValue || MinimalDate.Value <= parameter.OfferInfo.OfferStartTime
                && !MaximalDate.HasValue || MaximalDate.Value >= parameter.OfferInfo.OfferEndTime;
            } 
        }
    }
}
