using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AccommodationApplication.Controls
{
    /// <summary>
    /// Interaction logic for SimplePasswordBox.xaml
    /// </summary>
    public partial class SimplePasswordBox : UserControl
    {
        public SimplePasswordBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PasswordProperty=
            DependencyProperty.Register("Password", typeof(string), typeof(SimplePasswordBox), new FrameworkPropertyMetadata());

        public string Password
        {
            get { return (string) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value);}
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
        }
    }
}
