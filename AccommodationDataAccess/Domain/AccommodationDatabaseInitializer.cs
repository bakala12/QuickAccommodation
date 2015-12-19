using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

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
            foreach (var exampleRank in ExampleUsersProvider.GetExampleRanks())
            {
                context.Ranks.Add(exampleRank);
            }
            context.SaveChanges();
            foreach (var exampleUser in ExampleUsersProvider.GetExampleUsers())
            {
                context.Users.Add(exampleUser);
                Rank rank=context.Ranks.FirstOrDefault(r => r.Name.Equals("Nowicjusz"));
                exampleUser.Rank = rank;
            }
            context.SaveChanges();
        }
    }
}
