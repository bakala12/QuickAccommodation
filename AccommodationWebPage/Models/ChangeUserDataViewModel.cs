using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    /// <summary>
    /// Widok do zmiany danych użytkownika.
    /// </summary>
    public class ChangeUserDataViewModel
    {
        /// <summary>
        /// Imię (wymagane)
        /// </summary>
        [Display(Name = "Imię")]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko (wymagane)
        /// </summary>
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        /// <summary>
        /// Adres email (wymagane)
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Nazwa firmy (opcjonalne)
        /// </summary>
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Inicjalizuje nowy obiekt z pustymi właściwościami.
        /// </summary>
        public ChangeUserDataViewModel() { }

        /// <summary>
        /// Inicjalizuje nowy obiekt inicjalizując właściwości zgodnie z podanymi w modelu.
        /// </summary>
        /// <param name="model"></param>
        public ChangeUserDataViewModel(UserProfileViewModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            CompanyName = model.CompanyName;
            Email = model.Email;
        }
    }
}