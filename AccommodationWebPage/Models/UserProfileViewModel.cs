using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AccommodationDataAccess.Model;

namespace AccommodationWebPage.Models
{
    /// <summary>
    /// Model z danymi o uzytkowniku
    /// </summary>
    public class UserProfileViewModel
    {
        /// <summary>
        /// Nazwa użytkownika (wymagane)
        /// </summary>
        [Display(Name = "Nazwa użytkownika")]
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Ranga użytkownika (wymagane)
        /// </summary>
        [Display(Name = "Ranga")]
        [Required]
        public string Rank { get; set; }

        /// <summary>
        /// Imię użytkownika (wymagane)
        /// </summary>
        [Display(Name = "Imię")]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko użytkownika (wymagane)
        /// </summary>
        [Display(Name = "Nazwisko")]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Nazwa firmy (opcjonalna) 
        /// </summary>
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Adres email (wymagane)
        /// </summary>
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}