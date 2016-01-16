using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AccommodationDataAccess.Model;

namespace AccommodationWebPage.Models
{
    public class UserProfileViewModel
    {
        [Display(Name = "Nazwa użytkownika")]
        [Required]
        public string Username { get; set; }

        [Display(Name = "Ranga")]
        [Required]
        public string Rank { get; set; }

        [Display(Name = "Imię")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}