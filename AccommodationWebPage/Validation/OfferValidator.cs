using AccommodationWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccommodationWebPage.Validation
{
    public class OfferValidator
    {
        private AddNewOfferViewModel Model { get; set; }

        public OfferValidator(AddNewOfferViewModel model)
        {
            Model = new AddNewOfferViewModel
            {
                AccommodationName = model.AccommodationName,
                City = model.City,
                Description = model.Description,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                AvailiableVacanciesNumber = model.AvailiableVacanciesNumber,
                LocalNumber = model.LocalNumber,
                PostalCode = model.PostalCode,
                Price = model.Price,
                RoomNumber = model.RoomNumber,
                Street = model.Street
            };
        }

        /// <summary>
        /// sprawdza czy napis nie jest pusty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool ValidateBasicString(string str)
        {
            return !string.IsNullOrEmpty(str);
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

        public List<string> ValidateOffer()
        {
            List<string> errorList = new List<string>();
            if (!ValidateNumber(Model.AvailiableVacanciesNumber))
            {
                errorList.Add("Niepoprawna liczba miejsc");
            }
            if (ValidateNumber(Model.City))
            {
                errorList.Add("Niepoprawna nazwa miasta");
            }
            if (!ValidateDate(Model.StartDate, Model.EndDate))
            {
                errorList.Add("Niepoprawne daty");
            }
            if (!ValidateLocalNumber(Model.LocalNumber))
            {
                errorList.Add("Niepoprawny numer lokalu");
            }
            if (!ValidatePostalCode(Model.PostalCode))
            {
                errorList.Add("Niepoprawny kod pocztowy");
            }
            if (!ValidatePrice(Model.Price))
            {
                errorList.Add("Niepoprawna cena");
            }
            if (!ValidateNumber(Model.RoomNumber))
            {
                errorList.Add("Niepoprawny numer pokoju");
            }
            return errorList;
        }
    }
}