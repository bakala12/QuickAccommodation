using System;
using System.Collections.Generic;
using System.Linq;
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

    public interface ISearchingCriterion<in T> where T : Entity
    {
        SearchingCriterionType CriterionType { get; }
        bool IsGood(T parameter);
    }
}
