using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace UserAuthorizationSystem.Registration
{
    public interface IRegisterUser
    {
        User GetNewUser(string username, string clearTextPassword);
        void SaveUser<T>(User user, UserData userdata, Address address) where T:IUsersContext, IDisposable, new();
        Task SaveUserAsync<T>(User user, UserData userdata, Address address) where T:IUsersContext, IDisposable, new();
    }
}
