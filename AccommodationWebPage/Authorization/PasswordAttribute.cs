using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Authorization
{
    /// <summary>
    /// Atrybut do walidacji hasła.
    /// </summary>
    public class PasswordAttribute : StringLengthAttribute
    {
        /// <summary>
        /// Inicjalizuje nową instancję atrybutu PasswordAttribute.
        /// </summary>
        /// <param name="maximalLength">Minimalna długość hasła.</param>
        public PasswordAttribute(int maximalLength) : base(maximalLength)
        {
            MinimumLength = 8;
            DigitsMinimalAmount = 2;
            SpecialCharactersMinimalAmount = 0;
            CapitalLettersMinimalAmount = 0;
        }

        /// <summary>
        /// Zwraca lub ustawia minimalną liczbę cyfr w haśle.
        /// </summary>
        public uint DigitsMinimalAmount { get; set; }
        /// <summary>
        /// Zwraca lub ustawia minimalną liczbę znaków specjalnych w haśle.
        /// </summary>
        public uint SpecialCharactersMinimalAmount { get; set; }
        /// <summary>
        /// Zwraca lub ustawia minimalną liczbę wielkich liter w haśle.
        /// </summary>
        public uint CapitalLettersMinimalAmount { get; set; }

        /// <summary>
        /// Waliduje hasło.
        /// </summary>
        /// <param name="value">Obiekt reprezentujący hasło.</param>
        /// <returns>True, jeśli hasło jest poprawne, w przeciwnym wypadku false.</returns>
        public override bool IsValid(object value)
        {
            return base.IsValid(value) && IsValidPassword(value);
        }

        /// <summary>
        /// Validuje hasło.
        /// </summary>
        /// <param name="value">Obiekt reprezentujący hasło.</param>
        /// <returns>True jeśli hasło jest poprawne, w przeciwnym wypadku fałsz.</returns>
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