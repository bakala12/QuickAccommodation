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

        public bool ValidateEmail(string value)
        {
            return !string.IsNullOrEmpty(value) &&
                   Regex.IsMatch(value,
                       @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }
    }
}
