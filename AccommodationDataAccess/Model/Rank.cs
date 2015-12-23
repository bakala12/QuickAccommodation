using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    /// <summary>
    /// Model dla systemu rang
    /// </summary>
    public class Rank : Entity
    {
        /// <summary>
        /// Nazwa danej rangi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lista userów posiadających daną rangę
        /// </summary>
        public virtual IList<User> Users { get; set; } 
    }
}
