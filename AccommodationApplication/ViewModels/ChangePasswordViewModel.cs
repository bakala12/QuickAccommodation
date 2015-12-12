using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Services;
using AccommodationShared.Dtos;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    public class ChangePasswordViewModel : CloseableViewModel
    {
        private readonly string _username;
        private readonly IUserCredentialsValidator _validator;
        private readonly LoginProxy _loginService;
        private readonly UserProfileProxy _profileProxy;

        public ChangePasswordViewModel(string username)
        {
            _username = username;
            SavePasswordCommand = new DelegateCommand(async x=> await SavePasswordAsync(x));
            _validator = new UserCredentialsValidator();
            _loginService = new LoginProxy();
            _profileProxy = new UserProfileProxy();
        }

        public ICommand SavePasswordCommand { get; }

        private async Task SavePasswordAsync(object parameter)
        {
            PasswordBox[] passwords = parameter as PasswordBox[];
            if (passwords == null || passwords.Length!=3) throw new ArgumentException();
            string oldClearTextPassword = passwords[0].Password;
            string newPassword = passwords[1].Password;
            string newPasswordConfirmed = passwords[2].Password;
            string message;
            if (!_validator.ValidatePassword(newPassword, out message) && !_validator.ValidatePasswordConfirmation(newPassword, newPasswordConfirmed, out message))
            {
                Error = message;
                return;
            }
            CustomIdentity identity = await _loginService.GetUserAsync(_username, oldClearTextPassword);
            if (identity == null)
            {
                Error = "Nieprawidłowe hasło dla użytkownika " + _username;
                return;
            }
            UserNewPasswordDto dto = new UserNewPasswordDto()
            {
                Username = _username,
                Password = oldClearTextPassword,
                NewPassword = newPassword
            };
            try
            {
                await _profileProxy.ChangeUserPasswordAsync(dto);
                MessageBox.Show("Nowe hasło zostało pomyślnie zapisane.", "Hasło zmienione");
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd podczas zmiany hasła. Hasło nie zostało zmienione", "Błąd");
            }
            Close();
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }
    }
}
