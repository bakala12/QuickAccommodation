using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Login;
using AccommodationDataAccess.Domain;

namespace AccommodationApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            LoginCommand = new DelegateCommand(x => Login());
            RegisterCommand = new DelegateCommand(x=>Register());
        }

        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        protected virtual void Login()
        {
            LoginWindow login=new LoginWindow();
            LoginWindowViewModel vm=new LoginWindowViewModel();
            vm.RequestClose += (x,e)=>CloseWindow(login);
            login.DataContext = vm;
            login.ShowDialog();
        }

        protected virtual void Register()
        {
            RegisterWindow registerWindow = new RegisterWindow();
            RegiserNewUserViewModel vm = new RegiserNewUserViewModel();
            vm.RequestClose += (x, e) => CloseWindow(registerWindow);
            registerWindow.DataContext = vm;
            registerWindow.ShowDialog();
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }
    }
}
