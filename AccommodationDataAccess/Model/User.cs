using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    /// <summary>
    /// Model dla usera
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// Nazwa usera
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Zahaszowane hasło
        /// </summary>
        public string HashedPassword { get; set; }
        
        /// <summary>
        /// Sól do hasła
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Id informacji o userze w tabeli UserData
        /// </summary>
        public int UserDataId { get; set; }

        /// <summary>
        /// Informacje o userze
        /// </summary>
        public virtual UserData UserData { get; set; }
        
        /// <summary>
        /// Aktualna ranga
        /// </summary>
        public virtual Rank Rank { get; set; }

        /// <summary>
        /// Lista ofert wystawionych przez danego usera
        /// </summary>
        public virtual IList<Offer> MyOffers { get; set; }
        
        /// <summary>
        /// Lista ofert zarezerwowanych przez usera
        /// </summary>
        public virtual IList<Offer> PurchasedOffers { get; set; }

        /// <summary>
        /// Lista wszystkich ofert dodanych przez usera
        /// </summary>
        public virtual IList<HistoricalOffer> MyHistoricalOffers { get; set; } 
        
        /// <summary>
        /// Lista wszystkich zarezerwowanych ofert
        /// </summary>
        public virtual IList<HistoricalOffer> PurchasedHistoricalOffers { get; set; }
    
        /// <summary>
        /// Średnia ocena użytkownika
        /// </summary>
        public double? AverageMark { get; set; }

        /// <summary>
        /// Liczba ocen użytkownika
        /// </summary>
        public int? MarkCount { get; set; }

        public override string ToString()
        {
            return Username.ToString();
        }
    }
}
