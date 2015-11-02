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
            PasswordBox password = values[0] as PasswordBox;
            PasswordBox passwordConfirmed = values[1] as PasswordBox;
            if(password==null || passwordConfirmed==null) 
                throw new InvalidOperationException();
            return new PasswordBox[] {password, passwordConfirmed};
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
