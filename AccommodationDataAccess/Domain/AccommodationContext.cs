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
    public interface IUsersContext : IDisposable
    {
        IDbSet<User> Users { get; }
        int SaveChanges();
    }

    public interface IAccommodationContext : IDisposable
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Offer> Offers { get; set; }
        IDbSet<OfferInfo> OfferInfo { get; set; }
        IDbSet<Address> Addresses { get; set; }
        IDbSet<UserData> UserData { get; set; }
        IDbSet<Place> Places { get; set; }
        int SaveChanges();
    }

    public class AccommodationContext : DbContext, IUsersContext, IAccommodationContext
    {
        static AccommodationContext()
        {
            Database.SetInitializer<AccommodationContext>(new AccommodationDatabaseInitializer());
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Offer> Offers { get; set; }
        public IDbSet<OfferInfo> OfferInfo { get; set; }
        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<UserData> UserData { get; set; }
        public IDbSet<Place> Places { get; set; }
    }
}
