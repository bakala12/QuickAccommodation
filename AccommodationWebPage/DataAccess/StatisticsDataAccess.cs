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
    public class StatisticsDataAccess
    {
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
                model.MyReservedOfferAveragePrice =
                    context.HistoricalOffers.Where(o => o.CustomerId == user.Id)
                        .Include(o => o.OfferInfo)
                        .Average(o => o.OfferInfo.Price);
                model.AverageMyReservedOffersCountOnMonth = GetMyReservedOffersAverageCountOnMonth(context, user);
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StatisticsViewModel> GetUserStatisticsAsync(IAccommodationContext context,string username)
        {
            return await Task.Run(() => GetUserStatistics(context,username));
        }

        private int GetMyReservedOffersAverageCountOnMonth(IAccommodationContext context, User user)
        {
            List<HistoricalOffer> offers =
                context.HistoricalOffers.Where(o => o.CustomerId == user.Id).Include(o => o.OfferInfo).ToList();
            IDictionary<int, int> dict = new Dictionary<int, int>();
            foreach (var historicalOffer in offers)
            {
                int month = historicalOffer.OfferInfo.OfferStartTime.Month;
                int year = historicalOffer.OfferInfo.OfferStartTime.Year;
                int key = year*12 + month-1;
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, 1);
                }
                else
                {
                    dict[key] = dict[key] + 1;
                }
            }
            var order = dict.OrderBy(d => d.Key);
            int monthCount = order.Last().Key - order.First().Key;
            return offers.Count/monthCount;
        }
    }
}