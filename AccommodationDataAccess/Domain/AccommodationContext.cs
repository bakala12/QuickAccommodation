using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Domain
{
    public class AccommodationContext : DbContext
    {
        //static AccommodationContext()
        //{
        //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AccommodationContext>());
        //}
        public IDbSet<User> Users { get; set; } 
        public IDbSet<LoggedUser> LoggedUsers { get; set; }
        public IDbSet<Offer> Offers { get; set; }
        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<UserData> UserDatas { get; set; }
    }
}
