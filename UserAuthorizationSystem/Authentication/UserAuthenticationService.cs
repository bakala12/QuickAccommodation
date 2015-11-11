using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;

namespace UserAuthorizationSystem.Authentication
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        public CustomIdentity AuthenticateUser<T>(string username, string password) where T : IUsersContext, IDisposable, new()
        {
            using (var context = new T())
            {
                User user = context.Users.FirstOrDefault(x => x.Username.Equals(username));
                if (user == null) return null;
                string hash = PasswordHashHelper.CalculateHash(password, user.Salt);
                return user.HashedPassword.Equals(hash) ? new CustomIdentity(username) : null;
            }
        }

        public async Task<CustomIdentity> AuthenticateUserAsync<T>(string username, string password)
            where T : IUsersContext, IDisposable, new()
        {
            return await Task.Run(() => AuthenticateUser<T>(username, password));
        } 
    }
}
