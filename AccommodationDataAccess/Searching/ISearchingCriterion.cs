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
    /// Typ wyszukiwania.
    /// </summary>
    public enum SearchingCriterionType
    {
        /// <summary>
        /// Po cenie
        /// </summary>
        Price,
        /// <summary>
        /// Po dacie
        /// </summary>
        Date,
        /// <summary>
        /// Po miejscu
        /// </summary>
        Place,
        /// <summary>
        /// Wyszukiwanie zaawansowane
        /// </summary>
        Advanced
    }
    /// <summary>
    /// Kryterium wyszukiwania
    /// </summary>
    /// <typeparam name="T">Typ wyszukiwanego obiektu</typeparam>
    public interface ISearchingCriterion<T> where T : Entity
    {
        /// <summary>
        /// Typ kryterium wyszukiwania
        /// </summary>
        SearchingCriterionType CriterionType { get; }
        /// <summary>
        /// Wyrażenie podawane do LINQ realizujące odpowiednie wyszukiwanie.
        /// </summary>
        Expression<Func<T, bool>> SelectableExpression { get; } 
    }
}
