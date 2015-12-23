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
    /// <summary>
    /// Proxy dla ofert
    /// </summary>
    public class OffersProxy : WebApiProxy
    {
        public OffersProxy() : base("Offers", true)
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        }

        /// <summary>
        /// Zwraca ofertę o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Offer> GetOffer(int id)
        {
            return await this.Get<Offer>(string.Concat("GetOffer/", HttpUtility.UrlEncode(id.ToString())));
        }

        /// <summary>
        /// Zwraca już nieaktualną (przeniesioną do historii) ofertę o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Offer> GetHistoricalOffer(int id)
        {
            return await this.Get<Offer>(string.Concat("GetHistoricalOffer/", HttpUtility.UrlEncode(id.ToString())));
        }

        /// <summary>
        /// Pobiera aktualne oferty danego usera
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<Offer>> GetUserOffers(int userId)
        {
            return await this.Get<IList<Offer>>(string.Concat("GetUserOffers/", HttpUtility.UrlEncode(userId.ToString())));
        }


        /// <summary>
        /// Pobiera historię ofert danego użytkownika
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<HistoricalOffer>> GetUserHistoricalOffers(int userId)
        {
            return await this.Get<IList<HistoricalOffer>>(string.Concat("GetUserHistoricalOffers/", HttpUtility.UrlEncode(userId.ToString())));
        }

        /// <summary>
        /// Wysyła zapytanie dodające usera do bazy
        /// </summary>
        /// <param name="offerInfo"></param>
        /// <param name="vendor"></param>
        /// <param name="place"></param>
        /// <param name="room"></param>
        /// <returns></returns>
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

        internal Task RemoveHistoricalOfferAsync(string currentUser, int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Wysyła zapytanie do modyfikacji oferty
        /// </summary>
        /// <param name="username"></param>
        /// <param name="Id"></param>
        /// <param name="offerInfo"></param>
        /// <param name="place"></param>
        /// <param name="room"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Wysyła zapytanie o usunięcie oferty
        /// </summary>
        /// <param name="username"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task RemoveOfferAsync(string username, int Id)
        {
            OfferEditDataDto dataDto = new OfferEditDataDto()
            {
                Username = username,
                OfferId = Id
            };
            await Post<OfferEditDataDto, bool>("removeOffer", dataDto);
        }

        /// <summary>
        /// Wysyła zapytanie o zarezerwowanie danej oferty
        /// </summary>
        /// <param name="username"></param>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public async Task ReserveOffer(string username, int offerId)
        {
            ReserveOfferDto dto = new ReserveOfferDto()
            {
                Username = username,
                OfferId = offerId
            };
            await Post<ReserveOfferDto, bool>("reserve", dto);
        }

        /// <summary>
        /// Wysyła zapytanie o rezygnację z zarezerwowanej oferty
        /// </summary>
        /// <param name="username"></param>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public async Task ResignOffer(string username, int offerId)
        {
            ReserveOfferDto dto = new ReserveOfferDto()
            {
                Username = username,
                OfferId = offerId
            };
            await Post<ReserveOfferDto, bool>("resign", dto);
        }

        public async Task<IList<Offer>> GetReservedOffers(string username)
        {
            return await Get<IList<Offer>>("reservedOffers/" + HttpUtility.UrlEncode(username));
        }
    } 
}
