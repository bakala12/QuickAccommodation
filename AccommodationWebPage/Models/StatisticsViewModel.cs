using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    /// <summary>
    /// Model containing various users statistics
    /// </summary>
    public class StatisticsViewModel
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }
        [Required]
        [Display(Name="Ranga użytkownika")]
        public string Rank { get; set; }

        [Display(Name = "Najdroższa oferta")]
        public double MyMostValuableOffer { get; set; }
        [Display(Name = "Najtańsza oferta")]
        public double MyLessValuableOffer { get; set; }
        [Display(Name = "Liczba wystawionych ofert")]
        public int AllMyOffersCount { get; set; }
        [Display(Name = "Liczba obecnie wystawianych ofert")]
        public int MyOffersCountNow { get; set; }

        [Display(Name = "Liczba zarezerwowanych ofert")] 
        public int AllMyReservedOffersCount { get; set; }
        [Display(Name = "Liczba obecnie zarezerwowanych ofert")]
        public int MyReservedOffersCountNow { get; set; }

        public double[] ThisYearOffersPrices { get; set; }
        public double[] ThisYearReservedOffersPrices { get; set; }
        public int[] ThisYearOffersCountOnMonth { get; set; }
        public int[] ThisYearReservedOffersCountOnMonth { get; set; }

        public string[] Months { get; set; }
    }
}