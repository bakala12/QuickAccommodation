using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AccommodationContext())
            {
                User user = new User() {Login = "bakala12", Password = "****"};
                Address address = new Address()
                {
                    City = "Gołąb",
                    Street = "Piaskowa",
                    LocalNumber = "20",
                    PostalCode = "24-100"
                };
                UserData data = new UserData() {FirstName = "Mateusz", LastName = "Bąkała", CompanyName = "company", Address = address};
                LoggedUser loggedUser = new LoggedUser() {UserData = data, User = user};
                Offer offer = new Offer()
                {
                    Address = address,
                    OfferStartTime = new DateTime(2015, 10, 10),
                    OfferEndTime = new DateTime(2015, 10, 11),
                    Description = "Oferta",
                    Price = 1245.55,
                    AvailableVacanciesNumber = 3,
                    Vendor = loggedUser,
                    Customer = null
                };
                db.LoggedUsers.Add(loggedUser);
                db.SaveChanges();
            }
        }
    }
}
