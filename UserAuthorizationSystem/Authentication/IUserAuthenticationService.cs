using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using UserAuthorizationSystem.Identities;

namespace UserAuthorizationSystem.Authentication
{
    public interface IUserAuthenticationService
    {
        CustomIdentity AuthenticateUser<T>(string username, string password) where T : IUsersContext, IDisposable, new();

        Task<CustomIdentity> AuthenticateUserAsync<T>(string username, string password)
            where T : IUsersContext, IDisposable, new();
    }
}
