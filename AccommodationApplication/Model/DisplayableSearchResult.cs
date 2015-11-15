using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationApplication.Model
{
    public class DisplayableSearchResult : DisplayableOffer
    {
        public DisplayableSearchResult(Offer offer) : base(offer)
        {
            PlaceName = offer.Place.PlaceName;
        }

        public string PlaceName { get; set; }
    }
}
