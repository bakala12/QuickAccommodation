using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Models
{
    /// <summary>
    /// Model z danymi do przyznania oceny
    /// </summary>
    public class MarkViewModel
    {

        public MarkViewModel()
        {

        }
        public MarkViewModel(string username, int offerId)
        {
            Username = username;
            ReservedOfferId = offerId;
        }

        public string Username { get; set; }
        public int ReservedOfferId { get; set; }

        public Mark mark { get; set; }
    }

    public enum Mark
    {
        Okropnie = 1,
        Źle = 2,
        Średnio = 3,
        Dobrze = 4,
        Super = 5
    }
}