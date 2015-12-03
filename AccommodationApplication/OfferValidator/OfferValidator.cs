using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication
{
    /// <summary>
    /// Klasa do walidacji ofert
    /// </summary>
    public class OfferValidator
    {
        /// <summary>
        /// Zwraca informację czy Nazwa nie jest pusta
        /// </summary>
        /// <param name="value">nazwa miejsca</param>
        /// <returns></returns>
        public bool ValidateName(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Zwraca informację czy numer domu nie jest pusty i czy jego pierwszy znak jest liczbą
        /// </summary>
        /// <param name="value">numer domu</param>
        /// <returns></returns>
        public bool ValidateLocalNumber(string value)
        {
            return string.IsNullOrEmpty(value) || char.IsDigit(value[0]);
        }

        /// <summary>
        /// Walidacja kodu pocztowego
        /// </summary>
        /// <param name="value">kod pocztowy</param>
        /// <returns></returns>
        public bool ValidatePostalCode(string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            string[] s = value.Split('-');
            uint fir, sec;
            return s.Length == 2 && s[0].Length == 2 && s[1].Length == 3
                   && uint.TryParse(s[0], out fir) && uint.TryParse(s[1], out sec);
        }

        /// <summary>
        /// Zwraca informację czy data początkowa jest wcześniejsza niż data końcowa
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool ValidateDate(DateTime start, DateTime end)
        {
            return (start.Date.CompareTo(end.Date) <= 0);
        }

        /// <summary>
        /// Walidacja ceny
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool ValidatePrice(string price)
        {
            double p1;
            return double.TryParse(price, out p1) && double.Parse(price) >= 0 && char.IsDigit(price[0]);
        }

        /// <summary>
        /// Walidacja danych liczbowych (czy dadzą się zrzutować na int)
        /// </summary>
        /// <param name="vacancies"></param>
        /// <returns></returns>
        public bool ValidateNumber(string vacancies)
        {
            int p;
            return int.TryParse(vacancies, out p);
        }

    }
}
