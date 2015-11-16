using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Domain
{
    /// <summary>
    /// Zapewnia możliwośc dostarczenia kolekcji przykładowych użytkowników do bazy
    /// </summary>
    internal static class ExampleUsersProvider
    {
        /// <summary>
        /// Zwraca kolekcję przykładowych użytkowników
        /// </summary>
        /// <returns>Koleckja przykładowych użytkowników</returns>
        public static IEnumerable<User> GetExampleUsers()
        {
            List<User> users=new List<User>();
            User u = new User()
            {
                Username = "bakalam",
                HashedPassword = @"/TmCAkG+Rl0kJd+GvhgbVYghlOY3RXPFcRXLpx69sPk=",
                Salt = @"jQcO9f74p4sySJrXISk2XBw689JAPAic"
            };
            User u2 = new User()
            {
                Username = "jablonskim",
                HashedPassword = @"PIks6KBVlB0pE5DvlSmKpxKCs5tN9EDebq6s8bqb4HA=",
                Salt = @"zapt0VgkYldvUEc7K3TJ5S0aRLDavxmN"
            };
            UserData ud = new UserData()
            {
                Email = "bakalam@student.mini.pw.edu.pl",
                FirstName = "Mateusz",
                LastName = "Bąkała",
                CompanyName = "",
            };
            UserData ud2 = new UserData()
            {
                Email = "jablonskim2@student.mini.pw.edu.pl",
                FirstName = "Mateusz",
                LastName = "Jabłoński",
                CompanyName = ""
            };
            Address a = new Address()
            {
                City = "Gołąb",
                PostalCode = "24-100",
                Street = "Piaskowa",
                LocalNumber = "20"
            };
            Address a2 = new Address()
            {
                City = "Piotrków Trybunalski",
                Street = "",
                LocalNumber = "",
                PostalCode = ""
            };
            ud.Address = a;
            ud2.Address = a2;
            u.UserData = ud;
            u2.UserData = ud2;
            users.Add(u);
            users.Add(u2);
            User admin = new User()
            {
                Username = "admin",
                HashedPassword = @"HEsEsPd3IWOfCLXgPHXL4dA4zDPjOj/0Tltk5WOJ68A=",
                Salt = @"MpneiNAMrJyPySqkJqpnB/qNib5FrMa7"
            };
            UserData adminData = new UserData()
            {
                FirstName = "Jarosław",
                LastName = "Taboret",
                CompanyName = "Hotel SPA & Wellness",
                Email = @"admin@admin.pl"
            };
            Address add = new Address()
            {
                City = "Krajzegów",
                Street = "Kropelkowa",
                LocalNumber = "12a",
                PostalCode = "12-345",
            };
            adminData.Address = add;
            admin.UserData = adminData;
            users.Add(admin);
            return users;
        } 
    }
}
