using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public class OffersByPlaceSearchingCriterion : OfferSearchingCriterion
    {
        public string City { get; }
        public string PlaceName { get; }

        private OffersByPlaceSearchingCriterion() : base(SearchingCriterionType.Place) { }

        public OffersByPlaceSearchingCriterion(string city, string name =null) : this()
        {
            City = city;
            PlaceName = name;
        }

        public override bool IsGood(AvailableOffer parameter)
        {
            if(parameter?.OfferInfo?.Address == null) throw new ArgumentException("Niepełna informacja o ofercie");
            return !CheckCity(parameter.OfferInfo.Address) || string.IsNullOrEmpty(PlaceName) || CheckPlaceName(parameter.OfferInfo.Address);
        }

        private bool CheckCity(Address address)
        {
            if (string.IsNullOrEmpty(address?.City)) return false;
            return address.City.Equals(City);
        }

        private bool CheckPlaceName(Address address)
        {
            if (string.IsNullOrEmpty(address?.Name)) return false;
            return address.Name.Equals(PlaceName);
        }
    }
}
