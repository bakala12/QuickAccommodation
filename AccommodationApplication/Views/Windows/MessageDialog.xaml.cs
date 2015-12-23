using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using MahApps.Metro.Controls;

namespace AccommodationApplication.Views.Windows
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : MetroWindow
    {
        public MessageDialog()
        {
            InitializeComponent();
            CloseCommand = new DelegateCommand(x=>this.Close());
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageDialog), new PropertyMetadata(""));

        public ICommand CloseCommand { get; private set; }
    }
}
