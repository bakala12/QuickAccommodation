using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using AccommodationApplication.ViewModels;

namespace AccommodationApplication.Converter
{
    public class RegisterViewsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter?.ToString();
            CurrentScreen? current = value as CurrentScreen?;
            if (!current.HasValue) return Visibility.Collapsed;
            switch (param)
            {
                case "Credentials":
                    return current.Value == CurrentScreen.Credentials?Visibility.Visible:Visibility.Collapsed;
                case "BasicData":
                    return current.Value == CurrentScreen.BasicData?Visibility.Visible:Visibility.Collapsed;
                case "Address":
                    return current.Value == CurrentScreen.Address ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
