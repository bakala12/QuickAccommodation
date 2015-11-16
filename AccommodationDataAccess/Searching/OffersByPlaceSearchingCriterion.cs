using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    /// <summary>
    /// Implementacja kryterium wyszukiwania oferty po miejscu
    /// </summary>
    internal class OffersByPlaceSearchingCriterion : OfferSearchingCriterion
    {
        /// <summary>
        /// Nazwa miasta
        /// </summary>
        public string City { get; }
        /// <summary>
        /// Nazwa miejsca
        /// </summary>
        public string PlaceName { get; }

        /// <summary>
        /// Tworzy nową instancję kryterium wyszukiwania po miejscu
        /// </summary>
        private OffersByPlaceSearchingCriterion() : base(SearchingCriterionType.Place) { }

        /// <summary>
        /// Tworzy nową instancję kryterium wyszukiwanie po miejscu
        /// </summary>
        /// <param name="city">Nazwa miasta</param>
        /// <param name="name">Nazwa miejsca</param>
        public OffersByPlaceSearchingCriterion(string city, string name =null) : this()
        {
            City = city;
            PlaceName = name;
        }
        /// <summary>
        /// Wyrażenie podawane do LINQ realizujące odpowiednie wyszukiwanie.
        /// </summary>
        public override Expression<Func<Offer, bool>> SelectableExpression
        {
            get
            {
                return parameter =>
                    (string.IsNullOrEmpty(PlaceName) || parameter.Place.PlaceName.Equals(PlaceName)) &&
                    (string.IsNullOrEmpty(City) || parameter.Place.Address.City.Equals(City));
            }
        }
    }
}
