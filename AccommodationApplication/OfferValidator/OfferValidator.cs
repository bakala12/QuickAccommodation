﻿using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication
{
    public class OfferValidator
    {

        public bool ValidateName(string value)
        {
            return !string.IsNullOrEmpty(value);
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

        public bool ValidateDate(DateTime start, DateTime end)
        {
            return (start.Date.CompareTo(end.Date) < 0);
        }

        public bool ValidatePrice(string price)
        {
            int p1;
            return int.TryParse(price, out p1) ;
        }
        public bool ValidateNumber(string vacancies)
        {
            int p;
            return int.TryParse(vacancies, out p);
        }

    }
}
