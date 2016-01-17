using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AccommodationShared.Searching;

namespace AccommodationWebPage.Models
{
    /// <summary>
    /// Model bazowy dla zapytania
    /// </summary>
    public abstract class SearchingModel
    {
        public string Username { get; set; }
        [Display(Name = "Porządek sortowania")]
        public SortType SortType { get; set; }
        [Display(Name = "Sortuj wyniki według")]
        public SortBy SortBy { get; set; }
        public IList<OfferViewModel> Offers { get; set; }

        protected SearchingModel()
        {
            Offers = new List<OfferViewModel>();
        }
    }

    /// <summary>
    /// Model dla szukania po miejscu
    /// </summary>
    public class PlaceSearchingModel : SearchingModel
    {
        [Display(Name = "Nazwa miejsca")]
        public string PlaceName { get; set; }
        [Display(Name = "Nazwa miasta")]
        public string CityName { get; set; }
    }

    /// <summary>
    /// Model dla szukania po dacie
    /// </summary>
    public class DateSearchingModel : SearchingModel
    {
        [Display(Name = "Data rozpoczęcia")]
        public DateTime? MinimalDate { get; set; }
        public DateTime? MaximalDate { get; set; }
        public bool ShowPartiallyMatchingResults { get; set; }
    }

    /// <summary>
    /// Model dla szukania po cenie
    /// </summary>
    public class PriceSearchingModel : SearchingModel
    {
        public double? MinimalPrice { get; set; }
        public double? MaximalPrice { get; set; }
    }

    /// <summary>
    /// Model dla szukania zaawansowanego
    /// </summary>
    public class AdvancedSearchingModel : SearchingModel
    {
        public string PlaceName { get; set; }
        public string CityName { get; set; }
        public DateTime? MinimalDate { get; set; }
        public DateTime? MaximalDate { get; set; }
        public double? MinimalPrice { get; set; }
        public double? MaximalPrice { get; set; }
    }
}