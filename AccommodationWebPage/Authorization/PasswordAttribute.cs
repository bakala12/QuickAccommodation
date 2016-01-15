using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Authorization
{
    public class PasswordAttribute : StringLengthAttribute
    {
        public PasswordAttribute(int maximalLength) : base(maximalLength)
        {
            MinimumLength = 8;
            DigitsMinimalAmount = 2;
            SpecialCharactersMinimalAmount = 0;
            CapitalLettersMinimalAmount = 0;
        }

        public uint DigitsMinimalAmount { get; set; }
        public uint SpecialCharactersMinimalAmount { get; set; }
        public uint CapitalLettersMinimalAmount { get; set; }

        public override bool IsValid(object value)
        {
            return base.IsValid(value) && IsValidPassword(value);
        }

        private bool IsValidPassword(object value)
        {
            if (!(value is string)) return false;
            string password = value.ToString();
            return password.Length >= MinimumLength && password.Length <= MaximumLength &&
                   password.Count(char.IsDigit) >= DigitsMinimalAmount &&
                   password.Count(char.IsUpper) >= CapitalLettersMinimalAmount
                   && password.Count(c=>!char.IsLetterOrDigit(c))>=SpecialCharactersMinimalAmount;
        }
    }
}