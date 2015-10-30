using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace UserAuthorizationSystem.Validation
{
    public class NewUserDataValidator<T>: INewUserDataValidator
        where T: IUsersContext, IDisposable, new()
    {
        public virtual LoginValidationStatus ValidateUserLogin(string login)
        {
            if(string.IsNullOrEmpty(login) || login.Length<6)
                return new LoginValidationStatus(LoginValidationStatusEnumeration.TooShort, "Username must contain at least 6 characters");
            using (var db=new T())
            {
                bool b = db.Users.Any(x => login.Equals(x.Username));
                if(b) return new LoginValidationStatus(LoginValidationStatusEnumeration.AlreadyUsed, "This username is already used by another user");
            }
            return new LoginValidationStatus(LoginValidationStatusEnumeration.Correct, string.Empty);
        }

        public virtual PasswordValidationStatus ValidateUserPassword(string password)
        {
            if(string.IsNullOrEmpty(password))
                return new PasswordValidationStatus(PasswordValidationStatusEnumeration.TooShort, "Password cannot be empty");
            if(password.Length<8)
                return new PasswordValidationStatus(PasswordValidationStatusEnumeration.TooShort, "Password must contain at least 8 characters");
            if(password.ToCharArray().Count(char.IsDigit)<2)
                return new PasswordValidationStatus(PasswordValidationStatusEnumeration.TooWeak, "Password must contain at least two digits");
            return new PasswordValidationStatus(PasswordValidationStatusEnumeration.Correct, string.Empty);
        }

        public virtual PasswordValidationStatus ValidateUserPasswordConfirmed(string password, string password2)
        {
            if(string.IsNullOrEmpty(password2) || !password2.Equals(password))
                return new PasswordValidationStatus(PasswordValidationStatusEnumeration.ConfirmationFailed, "Passwords are not equal");
            return new PasswordValidationStatus(PasswordValidationStatusEnumeration.Correct, string.Empty);
        }

        public virtual bool ValidateUserEmail(string email)
        {
            return email != null &&
                   Regex.IsMatch(email,
                       @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }

        public virtual async Task<LoginValidationStatus> ValidateUserLoginAsync(string login)
        {
            if (string.IsNullOrEmpty(login) || login.Length < 6)
                return new LoginValidationStatus(LoginValidationStatusEnumeration.TooShort, "Username must contain at least 6 characters");
            using (var db = new T())
            {
                bool b = await db.Users.AnyAsync(x => login.Equals(x.Username));
                if (b) return new LoginValidationStatus(LoginValidationStatusEnumeration.AlreadyUsed, "This username is already used by another user");
            }
            return new LoginValidationStatus(LoginValidationStatusEnumeration.Correct, string.Empty);
        }
    }
}
