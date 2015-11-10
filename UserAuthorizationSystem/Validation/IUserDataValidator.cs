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

    /// <summary>
    /// Provides a way to validate email addresses.
    /// </summary>
    public interface IEmailValidator
    {
        /// <summary>
        /// Validates email address using regular expression.
        /// </summary>
        /// <param name="value">Email address to be validated.</param>
        /// <param name="reason">Output parameter which contains the reason of incorrect validation.</param>
        /// <returns>True if the given email address is valid, otherwise false. If method returns false,
        /// the reason of validation error is stored in output reason parameter.</returns>
        bool ValidateEmail(string value,out string reason);
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
