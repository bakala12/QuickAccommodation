using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Interfaces;
using AccommodationApplication.Services;
using AccommodationApplication.Views.Windows;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;

namespace AccommodationApplication.ViewModels
{
    public class MyProfileViewModel : ViewModelBase, IPageViewModel
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _companyName;
        private string _rank;
        private readonly UserProfileProxy _service;

        public MyProfileViewModel()
        {
            _service = new UserProfileProxy();
            ReloadData += async (x, e) => await LoadUserDataAsync();
            EditDataCommand = new DelegateCommand(x => EditData());
            ChangePasswordCommand = new DelegateCommand(x=>ChangePassword());
            ReloadData?.Invoke(this, EventArgs.Empty);
            (App.Current as App).Login += async (x, e) => await LoadUserDataAsync();
        }

        public string LoggedUser => Thread.CurrentPrincipal?.Identity?.Name;

        protected async virtual Task LoadUserDataAsync()
        {
            UserBasicDataDto data = await _service.GetUserAsync(LoggedUser);
            if (data == null) return;
            FirstName = data.FirstName;
            LastName = data.LastName;
            Email = data.Email;
            CompanyName = data.CompanyName ?? string.Empty;
            string r = await _service.GetUserRankAsync(LoggedUser);
            UserRank = r;
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string UserRank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                OnPropertyChanged();
            }
        }

        public string Name => "Mój profil";

        public event EventHandler ReloadData;

        public ICommand EditDataCommand { get; }
        public ICommand ChangePasswordCommand { get; }

        protected void EditData()
        {
            EditDataDialog dialog = new EditDataDialog();
            EditUserDataViewModel vm = new EditUserDataViewModel(LoggedUser)
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                CompanyName = CompanyName
            };
            dialog.DataContext = vm;
            vm.RequestClose += (x, e) => dialog.Close();
            dialog.ShowDialog();
            ReloadData?.Invoke(this, EventArgs.Empty); //email!
        }

        protected void ChangePassword()
        {
            ChangePasswordDialog dialog = new ChangePasswordDialog();
            ChangePasswordViewModel vm = new ChangePasswordViewModel(LoggedUser);
            dialog.DataContext = vm;
            vm.RequestClose += (x, e) => dialog.Close();
            dialog.ShowDialog();
        }
    }
}
