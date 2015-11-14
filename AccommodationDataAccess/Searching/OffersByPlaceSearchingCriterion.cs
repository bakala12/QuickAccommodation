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
    internal class OffersByPlaceSearchingCriterion : OfferSearchingCriterion
    {
        public string City { get; }
        public string PlaceName { get; }

        private OffersByPlaceSearchingCriterion() : base(SearchingCriterionType.Place) { }

        public OffersByPlaceSearchingCriterion(string city, string name =null) : this()
        {
            City = city;
            PlaceName = name;
        }

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
