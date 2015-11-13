using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public enum SearchingCriterionType
    {
        Price, Date, Place, VacanciesNumber
    }

    public enum ResultSortType
    {
        Ascending, Descending
    }

    public interface ISearchingCriterion<T> where T : Entity
    {
        SearchingCriterionType CriterionType { get; }
        Expression<Func<T, bool>> SelectableExpression { get; } 
    }
}
