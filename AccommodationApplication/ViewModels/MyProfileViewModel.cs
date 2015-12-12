using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public MyProfileViewModel()
        {
            ReloadData += async (x, e) => await LoadUserDataAsync();
            EditDataCommand = new DelegateCommand(x => EditData());
            ReloadData?.Invoke(this, EventArgs.Empty);
        }

        public string LoggedUser => Thread.CurrentPrincipal?.Identity?.Name;

        protected async virtual Task LoadUserDataAsync()
        {
            UserProfileProxy service = new UserProfileProxy();
            UserBasicDataDto data = await service.GetUserAsync(LoggedUser);
            if (data == null) return;
            FirstName = data.FirstName;
            LastName = data.LastName;
            Email = data.Email;
            CompanyName = data.CompanyName ?? string.Empty;
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
    }
}
