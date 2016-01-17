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
    /// <summary>
    /// Model do rejestracji uzytkownika
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Nazwa użytkownika (wymagane)
        /// </summary>
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        /// <summary>
        /// Hasło uzytkownika (wymagane, minimum 8 znaków w tym 2 cyfry)
        /// </summary>
        [Required]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [Password(100, MinimumLength = 8, CapitalLettersMinimalAmount = 0, 
            SpecialCharactersMinimalAmount = 0, DigitsMinimalAmount = 2,
            ErrorMessage = "Hasło powinno zawierać minimum 8 znaków i przynajmniej 2 cyfry")]
        public string Password { get; set; }

        /// <summary>
        /// Potwierdznie hasła (wymagane, musi być identyczne co hasło)
        /// </summary>
        [Required]
        [Display(Name="Potwierdź hasło")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Hasła nie są zgodne")]
        public string PasswordConfirmed { get; set; }

        /// <summary>
        /// Adres email (wymagane)
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Imię (wymagane)
        /// </summary>
        [Required]
        [Display(Name="Imię")]
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko (wymagane)
        /// </summary>
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        /// <summary>
        /// Nazwa firmy (opcjonalne)
        /// </summary>
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Ulica (opcjonalne)
        /// </summary>
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        /// <summary>
        /// Numer domu (opcjonalne)
        /// </summary>
        [Display(Name = "Numer domu")]
        public string LocalNumber { get; set; }

        /// <summary>
        /// Kod pocztowy (opcjonalny)
        /// </summary>
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Miejscowość (opcjonalna)
        /// </summary>
        [Display(Name = "Miejscowość")]
        public string City { get; set; }
    }
}