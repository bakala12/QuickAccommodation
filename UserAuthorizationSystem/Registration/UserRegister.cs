using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace UserAuthorizationSystem.Registration
{
    public class UserRegister : IRegisterUser
    {
        public User GetNewUser(string username, string clearTextPassword)
        {
            User user = new User();
            user.Username = username;
            string salt = PasswordHashHelper.CreateSalt();
            user.Salt = salt;
            user.HashedPassword = PasswordHashHelper.CalculateHash(clearTextPassword, salt);
            return user;
        }

        public void SaveUser<T>(User user, UserData userdata, Address address) where T : IUsersContext, IDisposable, new()
        {
            using (var context = new T())
            {
                using (var scope = new TransactionScope())
                {
                    user.UserData = userdata;
                    user.UserData.Address = address;
                    context.Users.Add(user);
                    scope.Complete();
                }
                context.SaveChanges();
            }
        }

        public async Task SaveUserAsync<T>(User user, UserData userdata, Address address) where T : IUsersContext, IDisposable, new()
        {
            await Task.Run(() => SaveUser<T>(user, userdata, address));
        }
    }
}
