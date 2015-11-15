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
    public class SortByValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SortByToStringConverter con= new SortByToStringConverter();
            IEnumerable<SortBy> val = value as IEnumerable<SortBy>;
            if (val == null) return null;
            List<string> str=new List<string>();
            foreach (var sortBy in val)
            {
                str.Add(con.Convert(sortBy, null,null,null)?.ToString());
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SortTypeValuesConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SortTypeToStringConverter con = new SortTypeToStringConverter();
            IEnumerable<SortType> val = value as IEnumerable<SortType>;
            if (val == null) return null;
            List<string> str = new List<string>();
            foreach (var sortBy in val)
            {
                str.Add(con.Convert(sortBy, null, null, null)?.ToString());
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
