using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Domain
{
    public class AccommodationDatabaseInitializer : CreateDatabaseIfNotExists<AccommodationContext>
    {
        public AccommodationDatabaseInitializer()
        {
        }

        protected override void Seed(AccommodationContext context)
        {
            foreach (var exampleUser in ExampleUsersProvider.GetExampleUsers())
            {
                context.Users.Add(exampleUser);
            }
            context.SaveChanges();
        }
    }
}
