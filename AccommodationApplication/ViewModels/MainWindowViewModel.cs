using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Login;

namespace AccommodationApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            LoginCommad = new DelegateCommand(x => Login());
        }

        public ICommand LoginCommad { get; set; }

        protected virtual void Login()
        {
            var login = new LoginWindow();
            LoginWindowViewModel vm = new LoginWindowViewModel();
            vm.RequestClose += (x, e) => CloseWindow(login);
            login.DataContext = vm;
            login.Show();
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }
    }
}
