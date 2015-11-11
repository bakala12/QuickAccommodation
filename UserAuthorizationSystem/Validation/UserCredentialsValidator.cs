using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;

namespace UserAuthorizationSystem.Validation
{
    public class UserCredentialsValidator : BasicUserDataValidator, IUserCredentialsValidator
    {
        public bool ValidateUsername<T>(string username) where T:IUsersContext, IDisposable, new()
        {
            using (var context=new T())
            {
                return !context.Users.Any(u => u.Username == username);
            }
        }

        public async Task<bool> ValidateUsernameAsync<T>(string username) where T : IUsersContext, IDisposable, new()
        {
            return await Task.Run(() => ValidateUsername<T>(username));
        }

        public virtual bool ValidatePassword(string password, out string reason)
        {
            reason=string.Empty;
            if (string.IsNullOrEmpty(password))
            {
                reason = "Hasło nie może byc puste";
                return false;
            }
            if (password.Length < 8)
            {
                reason = "Hasło musi zawierać przynajmniej 8 znaków";
                return false;
            }
            if (password.Count(char.IsDigit) < 2)
            {
                reason = "Hasło musi zawierać przynajmniej dwie cyfry";
                return false;
            }
            return true;
        }

        public virtual bool ValidatePasswordConfirmation(string password, string passwordConfirmed, out string reason)
        {
            reason = string.Empty;
            if (password == null || !password.Equals(passwordConfirmed))
            {
                reason = "Hasła nie są zgodne";
                return false;
            }
            return true;
        }
    }
}
