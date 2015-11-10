using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserAuthorizationSystem.Validation
{
    public class BasicUserDataValidator : IUserDataValidator, IEmailValidator
    {
        public bool ValidateName(string value)
        {
            return !string.IsNullOrEmpty(value) && char.IsLower(value[0]);
        }

        public bool ValidateLocalNumber(string value)
        {
            return string.IsNullOrEmpty(value) || char.IsDigit(value[0]);
        }

        public bool ValidatePostalCode(string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            string[] s = value.Split('-');
            uint fir, sec;
            return s.Length == 2 && s[0].Length == 2 && s[1].Length == 3
                   && uint.TryParse(s[0], out fir) && uint.TryParse(s[1], out sec);
        }

        /// <summary>
        /// Validates email address using regular expression.
        /// </summary>
        /// <param name="value">Email address to be validated.</param>
        /// <param name="reason">Output parameter which contains the reason of incorrect validation.</param>
        /// <returns>True if the given email address is valid, otherwise false. If method returns false,
        /// the reason of validation error is stored in output reason parameter.</returns>
        public bool ValidateEmail(string value, out string reason)
        {
            reason=string.Empty;
            if (string.IsNullOrEmpty(value))
            {
                reason = "Należy podac adres email";
                return false;
            }
            if (Regex.IsMatch(value,
                       @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                reason = "Nieprawidłowy adres email";
                return false;
            }
            return true;
        }
    }
}
