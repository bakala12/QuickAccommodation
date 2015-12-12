using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace AccommodationApplication.Converter
{
    public class PasswordsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<PasswordBox> passwordBoxes = new List<PasswordBox>();
            foreach (var value in values)
            {
                PasswordBox p = value as PasswordBox;
                if(p==null) throw new InvalidOperationException();
                passwordBoxes.Add(p);               
            }
            return passwordBoxes.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
