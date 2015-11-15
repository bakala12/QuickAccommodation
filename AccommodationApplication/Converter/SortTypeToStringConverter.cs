using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AccommodationApplication.ViewModels.SearchingViewModels;

namespace AccommodationApplication.Converter
{
    public class SortTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SortType? type = value as SortType?;
            if (!type.HasValue) return null;
            switch (type.Value)
            {
                case SortType.Ascending:
                    return "Rosnąco";
                case SortType.Descending:
                    return "Malejąco";
                case SortType.NotSort:
                    return "Brak sortowania";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "Rosnąco":
                    return SortType.Ascending;
                case "Malejąco":
                    return SortType.Descending;
                default:
                    return SortType.NotSort;
            }
        }
    }

    public class SortByToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SortBy? by = value as SortBy?;
            if (!by.HasValue) return null;
            switch (by.Value)
            {
                case SortBy.Place:
                    return "Nazwa miejsca";
                case SortBy.City:
                    return "Miasto";
                case SortBy.Price:
                    return "Cena";
                case SortBy.StartDate:
                    return "Data rozpoczęcia";
                case SortBy.EndDate:
                    return "Data zakończenia";
                case SortBy.VacanciesNumber:
                    return "Liczba wolnych miejsc";
                case SortBy.PublishDate:
                    return "Data opublikowania";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "Nazwa miejsca":
                    return SortBy.Place;
                case "Miasto":
                    return SortBy.City;
                case "Cena":
                    return SortBy.Price;
                case "Data rozpoczęcia":
                    return SortBy.StartDate;
                case "Data zakończenia":
                    return SortBy.EndDate;
                case "Data opublikowania":
                    return SortBy.PublishDate;
                default:
                    return SortBy.VacanciesNumber;
            }
        }
    }
}
