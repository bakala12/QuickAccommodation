using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    /// <summary>
    /// Model z danymi logowania użytkownika
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Nazwa uzytkownika (wymagana).
        /// </summary>
        [Required]
        [Display(Name = "Login")]
        public string Username { get; set; }

        /// <summary>
        /// Hasło uzytkownika (wymagane)
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}