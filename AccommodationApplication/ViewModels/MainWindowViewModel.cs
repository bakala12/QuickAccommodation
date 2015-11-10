using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Login;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using MahApps.Metro.Controls.Dialogs;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

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
            RegisterUserViewModel vm = new RegisterUserViewModel();
            vm.RequestClose += (x, e) => CloseWindow(registerWindow);
            registerWindow.DataContext = vm;
            registerWindow.ShowDialog();
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }

        public ObservableCollection<DisplayableUser> Users
        {
            get
            {
                var ret = new ObservableCollection<DisplayableUser>();
                //using (var db = new AccommodationContext())
                //{
                //    foreach (var user in db.Users)
                //    {
                //        ret.Add(new DisplayableUser(user, user.UserData));
                //    }
                //}
                return ret;
            }
        }

        public class DisplayableUser
        {
            public DisplayableUser(User user, UserData data)
            {
                Id = user.Id;
                Login = user.Username;
                FirstName = data.FirstName;
                CompanyName = data.CompanyName;
            }

            public int Id { get; set; }
            public string Login { get; set; }
            public string FirstName { get; set; }
            public string CompanyName { get; set; }
        }
    }
}
