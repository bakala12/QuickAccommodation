﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationWebPage.Models;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Registration;

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

        public async Task<string> ChangePasswordAsync(IAccommodationContext context, string username,
            ChangePasswordViewModel model)
        {
            try
            {
                IUserAuthenticationService authenticationService = new UserAuthenticationService();
                var identity = await authenticationService.AuthenticateUserAsync(context, username, model.OldPassword);
                if (identity == null)
                {
                    return "Nieprawidłowe hasło dla użytkownika " + username;
                }
                IRegisterUser register = new UserRegister();
                User newUser = register.GetNewUser(username, model.NewPassword);
                User user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null)
                {
                    return "Błąd! Nie odnaleziono użytkownika";
                }
                user.HashedPassword = newUser.HashedPassword;
                user.Salt = newUser.Salt;
                context.SaveChanges();
                return string.Empty;
            }
            catch (Exception)
            {
                return "Błąd!";
            }
        }
    }
}