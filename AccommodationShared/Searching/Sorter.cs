﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Rosnąco")]
        Ascending,
        [Display(Name = "Malejąco")]
        Descending,
        [Display(Name="Bez sortowania")]
        NotSort
    }

    /// <summary>
    /// Właściwość po której wyszukujemy
    /// </summary>
    public enum SortBy
    {
        [Display(Name = "Miejsce")]
        Place,
        [Display(Name = "Miasto")]
        City,
        [Display(Name = "Cena")]
        Price,
        [Display(Name = "Data rozpoczącia")]
        StartDate,
        [Display(Name = "Data zakończenia")]
        EndDate,
        [Display(Name = "Liczba miejsc")]
        VacanciesNumber,
        [Display(Name = "Data wystawienia")]
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
                    return o.OrderBy(x => x.Room.Place.PlaceName);
                case SortBy.City:
                    return o.OrderBy(x => x.Room.Place.Address.City);
                case SortBy.Price:
                    return o.OrderBy(x => x.OfferInfo.Price);
                case SortBy.StartDate:
                    return o.OrderBy(x => x.OfferInfo.OfferStartTime);
                case SortBy.EndDate:
                    return o.OrderBy(x => x.OfferInfo.OfferEndTime);
                case SortBy.VacanciesNumber:
                    return o.OrderBy(x => x.Room.Capacity);
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
                    return o.OrderByDescending(x => x.Room.Place.PlaceName);
                case SortBy.City:
                    return o.OrderByDescending(x => x.Room.Place.Address.City);
                case SortBy.Price:
                    return o.OrderByDescending(x => x.OfferInfo.Price);
                case SortBy.StartDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferStartTime);
                case SortBy.EndDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferEndTime);
                case SortBy.VacanciesNumber:
                    return o.OrderByDescending(x => x.Room.Capacity);
                case SortBy.PublishDate:
                    return o.OrderByDescending(x => x.OfferInfo.OfferPublishTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(@by), @by, null);
            }
        }
    }
}
