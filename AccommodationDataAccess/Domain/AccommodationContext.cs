using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Domain
{
    /// <summary>
    /// Interface with Users set, uses in logging and registering users functionality
    /// </summary>
    public interface IUsersContext : IDisposable
    {
        IDbSet<User> Users { get; }
        int SaveChanges();
    }

    /// <summary>
    /// Interface used as a database type
    /// </summary>
    public interface IAccommodationContext : IDisposable
    {
        IDbSet<User> Users { get; }
        IDbSet<Offer> Offers { get; }
        IDbSet<OfferInfo> OfferInfo { get; }
        IDbSet<Address> Addresses { get; }
        IDbSet<UserData> UserData { get; }
        IDbSet<Place> Places { get; }
        IDbSet<Rank> Ranks { get; }
        IDbSet<Room> Rooms { get; } 
        IDbSet<HistoricalOffer> HistoricalOffers { get; } 
        int SaveChanges();
    }

    public class AccommodationContext : DbContext, IUsersContext, IAccommodationContext
    {
        static AccommodationContext()
        {
            Database.SetInitializer<AccommodationContext>(new AccommodationDatabaseInitializer());
        }

        public AccommodationContext()
           // : base("AccomConnStr") //for tests I Choose local db
        {
            
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Offer> Offers { get; set; }
        public IDbSet<OfferInfo> OfferInfo { get; set; }
        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<UserData> UserData { get; set; }
        public IDbSet<Place> Places { get; set; }
        public IDbSet<Room> Rooms { get; set; }
        public IDbSet<Rank> Ranks { get; set; } 
        public IDbSet<HistoricalOffer> HistoricalOffers { get; set; } 
    }
}
