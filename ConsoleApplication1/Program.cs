using System;
using System.Collections.Generic;
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
                UserData data = new UserData() {FirstName = "Mateusz", LastName = "Bąkała", CompanyName = "company"};
                LoggedUser loggedUser = new LoggedUser() {UserData = data, User = user};
                db.LoggedUsers.Add(loggedUser);
                db.UserDatas.Add(data);
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}
