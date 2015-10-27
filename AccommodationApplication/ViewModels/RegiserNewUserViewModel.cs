using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationApplication.Commands;

namespace AccommodationApplication.ViewModels
{
    public class RegiserNewUserViewModel : LoginWindowViewModel, IDataErrorInfo
    {
        private string _password2;
        private string _email;

        public RegiserNewUserViewModel()
        {
            RegisterCommand=new DelegateCommand(x=>Register());
        }

        public string Password2
        {
            get { return _password2; }
            set { _password2 = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public DelegateCommand RegisterCommand { get; set; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    default:
                        break;
                }
                return string.Empty;
            }
        }

        public string Error { get; } = string.Empty;

        protected virtual void Register()
        {
            //Register logic here
            Close();
        }
    }
}
