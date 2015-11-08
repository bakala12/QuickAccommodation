﻿using System;
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
    }

    public class AccommodationContext : DbContext, IUsersContext
    {

        public AccommodationContext()
        {
            Database.SetInitializer<AccommodationContext>(new CreateDatabaseIfNotExists<AccommodationContext>());
        }

        public IDbSet<User> Users { get; set; } 
        public IDbSet<Offer> Offers { get; set; }
        public IDbSet<OfferInfo> OfferInfo { get; set; } 
        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<UserData> UserData { get; set; }
    }
}
