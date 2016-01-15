using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI.WebControls;
using AccommodationWebPage.Authorization;

namespace AccommodationWebPage.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [Password(100, MinimumLength = 6, CapitalLettersMinimalAmount = 0, 
            SpecialCharactersMinimalAmount = 0, DigitsMinimalAmount = 2,
            ErrorMessage = "Hasło powinno zawierać minimum 8 znaków i przynajmniej 2 cyfry")]
        public string Password { get; set; }

        [Required]
        [Display(Name="Potwierdź hasło")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Hasła nie są zgodne")]
        public string PasswordConfirmed { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name="Imię")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Numer domu")]
        public string LocalNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Display(Name = "Miejscowość")]
        public string City { get; set; }
    }
}