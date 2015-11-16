using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;

namespace UserAuthorizationSystem.Validation
{
    /// <summary>
    /// Provides a way to validate basic user data.
    /// </summary>
    public interface IUserDataValidator
    {
        /// <summary>
        /// Validate the name of the user.
        /// </summary>
        /// <param name="value">The value to be validated</param>
        /// <returns>True if the value is valid otherwise false</returns>
        bool ValidateName(string value);
        /// <summary>
        /// Validates user local number
        /// </summary>
        /// <param name="value">Local number to be validated</param>
        /// <returns></returns>
        bool ValidateLocalNumber(string value);
        /// <summary>
        /// Validates the postal code.
        /// </summary>
        /// <param name="value">Postal code to be validated</param>
        /// <returns>True if the value is valid otherwise false</returns>
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

    /// <summary>
    /// Provides a way to validate passwords.
    /// </summary>
    public interface IPasswordValidator
    {
        /// <summary>
        /// Validates the password
        /// </summary>
        /// <param name="password">Password to be validated</param>
        /// <param name="reason">Reason of error, may be empty</param>
        /// <returns>True if the value is valid otherwise false</returns>
        bool ValidatePassword(string password, out string reason);
        /// <summary>
        /// Validates the password and its confirmation
        /// </summary>
        /// <param name="password">Password to be validated</param>
        /// <param name="passwordConfirmed">Password confirmation</param>
        /// <param name="reason">Reason of error, may be empty</param>
        /// <returns>True if the value is valid otherwise false</returns>
        bool ValidatePasswordConfirmation(string password, string passwordConfirmed, out string reason);
    }

    /// <summary>
    /// Extends email validation and password validation with validation of advanced user data. 
    /// </summary>
    public interface IUserCredentialsValidator : IEmailValidator, IUserDataValidator, IPasswordValidator
    {
        /// <summary>
        /// Validates the username. 
        /// </summary>
        /// <typeparam name="T">The database type.</typeparam>
        /// <param name="username">The username to be validated</param>
        /// <returns>True if the value is valid otherwise false</returns>
        bool ValidateUsername<T>(string username) where T : IUsersContext, IDisposable, new();
        /// <summary>
        /// Validates the username. 
        /// </summary>
        /// <typeparam name="T">The database type.</typeparam>
        /// <param name="username">The username to be validated</param>
        /// <returns>True if the value is valid otherwise false</returns>
        Task<bool> ValidateUsernameAsync<T>(string username) where T : IUsersContext, IDisposable, new();
    }
}
