using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Domain
{
    /// <summary>
    /// Inicjalizator bazy danych wstawiający do niej przykładowych użytkowników
    /// </summary>
    public class AccommodationDatabaseInitializer : CreateDatabaseIfNotExists<AccommodationContext>
    {
        /// <summary>
        /// Wstawia przykładowych użytkowników do bazy
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
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
