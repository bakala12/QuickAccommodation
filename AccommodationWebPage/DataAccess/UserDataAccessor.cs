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
                if (user == null) return null;
                UserData data = context.UserData.FirstOrDefault(ud => ud.Id == user.UserDataId);
                if (data == null) return null;
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

        public async Task<UserProfileViewModel> GetInfoAboutUserAsync(IAccommodationContext context, string username)
        {
            return await Task.Run(() => GetInfoAboutUser(context, username));
        }

        public bool SaveUserData(IAccommodationContext context, string username, ChangeUserDataViewModel model)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return false;
                var data = context.UserData.FirstOrDefault(ud => ud.Id == user.UserDataId);
                if (data == null) return false;
                data.Email = model.Email;
                data.CompanyName = model.CompanyName;
                data.FirstName = model.FirstName;
                data.LastName = model.LastName;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SaveUserDataAsync(IAccommodationContext context, string username,
            ChangeUserDataViewModel model)
        {
            return await Task.Run(() => SaveUserData(context, username, model));
        }
    }
}