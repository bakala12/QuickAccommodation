using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccommodationApplication.Services
{
    public class OffersProxy : WebApiProxy
    {
        public OffersProxy() : base("Offers", true)
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        }

        public async Task<Offer> Get(int id)
        {
            return await this.Get<Offer>(id.ToString());
        }

        public async Task<IList<Offer>> GetUserOffers(int userId)
        {
            return await this.Get<IList<Offer>>(string.Concat("GetUserOffers/", HttpUtility.UrlEncode(userId.ToString())));
        }

        public async Task SaveOfferAsync(OfferInfo offerInfo, User vendor, Place place, Room room)
        {
            OfferAllDataDto dto = new OfferAllDataDto()
            {
                Vendor = vendor,
                OfferInfo = offerInfo,
                Place = place,
                Room = room
            };
            await Post<OfferAllDataDto, object>("saveOffer", dto);
        }

        public async Task EditOfferAsync(string username, int Id, OfferInfo offerInfo, Place place, Room room)
        {
            OfferEditDataDto dataDto = new OfferEditDataDto()
            {
                OfferInfo = offerInfo,
                Place = place,
                Username = username,
                OfferId = Id,
                Room = room
            };
            await Post<OfferEditDataDto, bool>("editOffer", dataDto);
        }

        public async Task RemoveOfferAsync(string username, int Id)
        {
            OfferEditDataDto dataDto = new OfferEditDataDto()
            {
                Username = username,
                OfferId = Id
            };
            await Post<OfferEditDataDto, bool>("removeOffer", dataDto);
        }

        public async Task ReserveOffer(string username, int offerId)
        {
            ReserveOfferDto dto = new ReserveOfferDto()
            {
                Username = username,
                OfferId = offerId
            };
            await Post<ReserveOfferDto, bool>("reserve", dto);
        }

        public async Task ResignOffer(string username, int offerId)
        {
            ReserveOfferDto dto = new ReserveOfferDto()
            {
                Username = username,
                OfferId = offerId
            };
            await Post<ReserveOfferDto, bool>("resign", dto);
        }
    }
}
