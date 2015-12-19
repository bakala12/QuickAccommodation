using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationShared.Searching
{
    /// <summary>
    /// Typ sortownia 
    /// </summary>
    public enum SortType
    {
        Ascending,
        Descending,
        NotSort
    }

    /// <summary>
    /// Właściwość po której wyszukujemy
    /// </summary>
    public enum SortBy
    {
        Place,
        City,
        Price,
        StartDate,
        EndDate,
        VacanciesNumber,
        PublishDate
    }

    /// <summary>
    /// Rozszerza interfejs IQueryable o sortowanie zdefiniowane przez SortType i SortBy
    /// </summary>
    public static class Sorter
    {
        public static IQueryable<Offer> OrderBy(this IQueryable<Offer> o, SortType type, SortBy by)
        {
            if (type == SortType.Ascending) return o.OrderBy(by);
            if (type == SortType.Descending) return o.OrderByDescending(by);
            return o;
        }

        private static IQueryable<Offer> OrderBy(this IQueryable<Offer> o, SortBy by)
        {
            switch (by)
            {
                case SortBy.Place:
                    return o.OrderBy(x => x.Place.PlaceName);
                case SortBy.City:
                    return o.OrderBy(x => x.Place.Address.City);
                case SortBy.Price:
                    return o.OrderBy(x => x.OfferInfo.Price);
                case SortBy.StartDate:
                    return o.OrderBy(x => x.OfferInfo.OfferStartTime);
                case SortBy.EndDate:
                    return o.OrderBy(x => x.OfferInfo.OfferEndTime);
                case SortBy.VacanciesNumber:
                    return o.OrderBy(x => x.OfferInfo.AvailableVacanciesNumber);
                case SortBy.PublishDate:
                    return o.OrderBy(x => x.OfferInfo.OfferPublishTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(@by), @by, null);
            }
        }

        private static IQueryable<Offer> OrderByDescending(this IQueryable<Offer> o, SortBy by)
        {
            switch (by)
            {
                case SortBy.Place:
                    return o.OrderByDescending(x => x.Place.PlaceName);
                case SortBy.City:
                    return o.OrderByDescending(x => x.Place.Address.City);
                case SortBy.Price:
                    return o.OrderByDescending(x => x.OfferInfo.Price);
                case SortBy.StartDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferStartTime);
                case SortBy.EndDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferEndTime);
                case SortBy.VacanciesNumber:
                    return o.OrderByDescending(x => x.OfferInfo.AvailableVacanciesNumber);
                case SortBy.PublishDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferPublishTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(@by), @by, null);
            }
        }
    }
}
