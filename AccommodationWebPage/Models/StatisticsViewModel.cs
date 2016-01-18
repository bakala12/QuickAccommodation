using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    public class StatisticsViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Rank { get; set; }

        public double MyMostValuableOffer { get; set; }

        public double MyLessValuableOffer { get; set; }

        public int AllMyOffersCount { get; set; }

        public int AllMyReservedOffersCount { get; set; }

        public int MyOffersCountNow { get; set; }

        public int MyReservedOffersCountNow { get; set; }

        public double AverageMyReservedOffersCountOnMonth { get; set; }

        public double MyReservedOfferAveragePrice { get; set; }
    }
}