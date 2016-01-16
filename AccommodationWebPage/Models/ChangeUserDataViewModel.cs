using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    public class ChangeUserDataViewModel
    {
        [Display(Name = "Imię")]
        [Required]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        public ChangeUserDataViewModel() { }

        public ChangeUserDataViewModel(UserProfileViewModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            CompanyName = model.CompanyName;
            Email = model.Email;
        }
    }
}