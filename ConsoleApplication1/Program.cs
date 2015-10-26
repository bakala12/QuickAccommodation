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
                db.Users.Add(new User() {Login = "bakala12", Password = "*******"});
                db.SaveChanges();
            }
        }
    }
}
