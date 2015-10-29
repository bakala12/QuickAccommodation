using System;
using System.Collections.Generic;
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
    }


    public class AccommodationContext : DbContext, IUsersContext
    {
        public IDbSet<User> Users { get; set; } 
        public IDbSet<LoggedUser> LoggedUsers { get; set; }
        public IDbSet<Offer> Offers { get; set; }
        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<UserData> UserData { get; set; }
    }
}
