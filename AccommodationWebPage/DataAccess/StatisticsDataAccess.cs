using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationWebPage.Models;

namespace AccommodationWebPage.DataAccess
{
    /// <summary>
    /// Gets the statistics for user
    /// </summary>
    public class StatisticsDataAccess
    {
        /// <summary>
        /// Gets users statistics
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="username">Name of the user</param>
        /// <returns>Model filled with statistics</returns>
        public StatisticsViewModel GetUserStatistics(IAccommodationContext context,string username)
        {
            try
            {
                User user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return null;
                StatisticsViewModel model = new StatisticsViewModel();
                model.Username = username;
                model.Rank = user.Rank.Name;
                model.AllMyOffersCount = context.HistoricalOffers.Count(o => o.VendorId == user.Id);
                model.AllMyReservedOffersCount = context.HistoricalOffers.Count(o => o.CustomerId == user.Id);
                model.MyOffersCountNow = context.Offers.Count(o => o.VendorId == user.Id);
                model.MyReservedOffersCountNow = context.Offers.Count(o => o.CustomerId == user.Id);
                List<HistoricalOffer> myOffers =
                    context.HistoricalOffers.Where(o => o.VendorId == user.Id).Include(o => o.OfferInfo).ToList();
                model.MyLessValuableOffer = myOffers.Min(o => o.OfferInfo.Price);
                model.MyMostValuableOffer = myOffers.Max(o => o.OfferInfo.Price);
                int[] counts;
                model.ThisYearOffersPrices = ThisYearOfferPrices(context, user, out counts);
                model.ThisYearOffersCountOnMonth = counts;
                int[] counts2;
                model.ThisYearReservedOffersPrices = ThisYearReservedOffersPrices(context, user, out counts2);
                model.ThisYearReservedOffersCountOnMonth = counts2;
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Asynchronously gets user statistics
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="username">Name of the user</param>
        /// <returns>Model filled with statistics</returns>
        public async Task<StatisticsViewModel> GetUserStatisticsAsync(IAccommodationContext context,string username)
        {
            return await Task.Run(() => GetUserStatistics(context,username));
        }

        private double[] ThisYearOfferPrices(IAccommodationContext context, User user, out int[] counts)
        {
            int year = DateTime.Now.Year;
            double[] prices = new double[12];
            counts = new int[12];
            var offers =
                context.HistoricalOffers.Where(o => o.VendorId == user.Id)
                    .Include(o => o.OfferInfo)
                    .Where(o => o.OfferInfo.OfferStartTime.Year == year).ToList();
            foreach (var offer in offers)
            {
                int month = offer.OfferInfo.OfferStartTime.Month-1;
                prices[month] = (prices[month]*counts[month] + offer.OfferInfo.Price)/(counts[month] + 1);
                counts[month]++;
            }
            return prices;
        }

        private double[] ThisYearReservedOffersPrices(IAccommodationContext context, User user, out int[] counts)
        {
            int year = DateTime.Now.Year;
            double[] prices = new double[12];
            counts = new int[12];
            var offers =
                context.HistoricalOffers.Where(o => o.CustomerId == user.Id)
                    .Include(o => o.OfferInfo)
                    .Where(o => o.OfferInfo.OfferStartTime.Year == year).ToList();
            foreach (var offer in offers)
            {
                int month = offer.OfferInfo.OfferStartTime.Month - 1;
                prices[month] = (prices[month] * counts[month] + offer.OfferInfo.Price) / (counts[month] + 1);
                counts[month]++;
            }
            return prices;
        }
    }
}