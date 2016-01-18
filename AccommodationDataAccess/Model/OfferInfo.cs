using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public class OfferInfo : Entity
    {
        /// <summary>
        /// Data początkowa oferty
        /// </summary>
        public DateTime OfferStartTime { get; set; }

        /// <summary>
        /// Data końcowa oferty
        /// </summary>
        public DateTime OfferEndTime { get; set; }

        /// <summary>
        /// Opis oferty
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Cena
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Data publikacji oferty
        /// </summary>
        public DateTime OfferPublishTime { get; set; }

        /// <summary>
        /// Zdjęcie oferty
        /// </summary>
        public byte[] OfferImage { get; set; }
    }
}
