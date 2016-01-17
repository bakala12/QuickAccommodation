using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    public class AddNewOfferViewModel
    {
        public AddNewOfferViewModel()
        {

        }
        public AddNewOfferViewModel(Offer offer)
        {
            StartDate = offer.OfferInfo.OfferStartTime;
            EndDate = offer.OfferInfo.OfferEndTime;
            Street = offer.Room.Place.Address.Street;
            LocalNumber = offer.Room.Place.Address.LocalNumber;
            PostalCode = offer.Room.Place.Address.PostalCode;
            City = offer.Room.Place.Address.City;
            AccommodationName = offer.Room.Place.PlaceName;
            AvailiableVacanciesNumber = String.Format("{0}",offer.Room.Capacity);
            RoomNumber = offer.Room.Number;
            Price = String.Format("{0}",offer.OfferInfo.Price);
            Description = offer.OfferInfo.Description;
            Id = offer.Id;
        }

        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Cena")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Data początkowa")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Data końcowa")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string AccommodationName { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Numer domu")]
        public string LocalNumber { get; set; }

        [Required]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Numer pokoju")]
        public string RoomNumber { get; set; }

        [Required]
        [Display(Name = "Liczba wolnych miejsc")]
        public string AvailiableVacanciesNumber { get; set; }

        public int Id { get; set; }


    }
}