using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;

namespace UserAuthorizationSystem.Validation
{
    public interface IUserDataValidator
    {
        bool ValidateName(string value);
        bool ValidateLocalNumber(string value);
        bool ValidatePostalCode(string value);
    }

    public interface IEmailValidator
    {
        bool ValidateEmail(string value);
    }

    public interface IPasswordValidator
    {
        bool ValidatePassword(string password, out string reason);
        bool ValidatePasswordConfirmation(string password, string passwordConfirmed);
    }

    public interface IUserCredentialsValidator : IEmailValidator, IUserDataValidator, IPasswordValidator
    {
        bool ValidateUsername<T>(string username) where T : IUsersContext, IDisposable, new();
        Task<bool> ValidateUsernameAsync<T>(string username) where T : IUsersContext, IDisposable, new();
    }
}
