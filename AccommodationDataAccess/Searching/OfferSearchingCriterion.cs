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
    /// Stanowi bazową abstrakcję dla kryterium wyszukiwania ofert.
    /// </summary>
    public abstract class OfferSearchingCriterion : ISearchingCriterion<Offer>
    {
        /// <summary>
        /// Inicjalizuje właściwości obiektu
        /// </summary>
        /// <param name="criterionType"></param>
        protected OfferSearchingCriterion(SearchingCriterionType criterionType)
        {
            CriterionType = criterionType;
        }

        /// <summary>
        /// Typ kryterium wyszukiwania
        /// </summary>
        public SearchingCriterionType CriterionType { get; }
        /// <summary>
        /// Wyrażenie podawane do LINQ realizujące odpowiednie wyszukiwanie.
        /// </summary>
        public abstract Expression<Func<Offer, bool>> SelectableExpression { get; }
    }
}
