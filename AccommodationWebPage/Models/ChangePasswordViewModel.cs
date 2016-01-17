using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AccommodationWebPage.Authorization;

namespace AccommodationWebPage.Models
{
    /// <summary>
    /// Model do zmiany hasła użytkownika.
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Stare hasło (wymagane)
        /// </summary>
        [Required]
        [Display(Name = "Stare hasło")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        /// <summary>
        /// Nowe hasło (wymagane, minimum 8 znaków w tym 2 cyfry)
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Password(100, MinimumLength = 8, CapitalLettersMinimalAmount = 0,
            SpecialCharactersMinimalAmount = 0, DigitsMinimalAmount = 2,
            ErrorMessage = "Hasło powinno zawierać minimum 8 znaków i przynajmniej 2 cyfry")]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Potwierdzenie nowego hasła (wymagane, musi byc zgodne z nowym hasłem)
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne")]
        public string NewPasswordConfirmed { get; set; }
    }
}