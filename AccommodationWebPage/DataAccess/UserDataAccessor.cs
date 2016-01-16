using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationWebPage.Models;

namespace AccommodationWebPage.DataAccess
{
    public class UserDataAccessor
    {
        public UserProfileViewModel GetInfoAboutUser(IAccommodationContext context, string username)
        {
            try
            {
                User user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                UserData data = context.UserData.FirstOrDefault(ud => ud.Id == user.UserDataId);
                if (user == null || data == null) return null;
                UserProfileViewModel vm = new UserProfileViewModel()
                {
                    Username = user.Username,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    CompanyName = data.CompanyName,
                    Email = data.Email,
                    Rank = user.Rank.Name
                };
                return vm;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SaveUserDataAsync(IAccommodationContext context, UserProfileViewModel model)
        {
            return true;
        }
    }
}