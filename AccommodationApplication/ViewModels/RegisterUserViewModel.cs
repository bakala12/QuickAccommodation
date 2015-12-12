using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AccommodationApplication.Commands;
using AccommodationApplication.Services;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using UserAuthorizationSystem.Registration;
using UserAuthorizationSystem.Validation;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// Enum potrzebny do zmiany ekranów w oknie rejestracji
    /// </summary>
    public enum CurrentScreen
    {
        Credentials, BasicData, Address
    }

    /// <summary>
    /// ViewModel odpowiedzialny za rejestrację nowego użytkownika
    /// </summary>
    public class RegisterUserViewModel : CloseableViewModel
    {
        private string _username;
        private string _email;

        private string _firstName;
        private string _lastName;
        private string _companyName;

        private string _street;
        private string _localeNumber;
        private string _postalCode;
        private string _city;

        private string _error;
        private CurrentScreen _currentScreen;
        private readonly IUserCredentialsValidator _validator;
        private readonly LoginProxy _service;
        private User _user;
        private UserData _userData;
        private Address _address;

        /// <summary>
        /// Inicjalizuje nową instancję klasy RegisterUserViewModel
        /// </summary>
        /// <param name="validator">Validator danych wprowadzanych przez użytkownika</param>
        /// <param name="regiser">Instancja odpowiadająca za zapisanie użytkownika do bazy danych</param>
        public RegisterUserViewModel(IUserCredentialsValidator validator)
        {
            if(validator == null) throw new ArgumentNullException();
            _validator = validator;
            _service = new LoginProxy();
            NextCommand = new DelegateCommand(async x=> await NextScreenAsync(x));
            RegisterCommand = new DelegateCommand(async x =>await RegisterAsync());
            CurrentScreen = CurrentScreen.Credentials;
        }

        /// <summary>
        /// Komenda odpowiadająca za przejście do następnego ekranu
        /// </summary>
        public ICommand NextCommand { get; }
        /// <summary>
        /// Komenda odpowiadająca na zlecenie zarejestrowania użytkownika
        /// </summary>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// Pobiera lub ustawia aktualnie wyświetlany ekran
        /// </summary>
        public CurrentScreen CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                _currentScreen = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Asynchronicznie przechodzi do następnego ekranu
        /// </summary>
        /// <param name="x">Parametr komendy</param>
        /// <returns></returns>
        public async Task NextScreenAsync(object x)
        {
            Error = null;
            switch (CurrentScreen)
            {
                case CurrentScreen.Credentials:
                    PasswordBox[] passwords = x as PasswordBox[];
                    if(passwords==null || passwords.Length!=2) throw new ArgumentException();
                    string err;
                    if (string.IsNullOrEmpty(Username))
                    {
                        Error = "Podaj nazwę użytkownika";
                        return;
                    }
                    bool b = await _validator.ValidateUsernameAsync<AccommodationContext>(Username);
                    if (!b)
                    {
                        Error = "Nazwa użytkownika jest już zajęta";
                        return;
                    }
                    if (!_validator.ValidateEmail(Email, out err) || 
                        !_validator.ValidatePassword(passwords[0].Password, out err) || 
                        !_validator.ValidatePasswordConfirmation(passwords[0].Password, passwords[1].Password, out err))
                    {
                        Error = err;
                        return;
                    }
                    _user = await _service.GetNewUserAsync(Username, passwords[0].Password);
                    _userData = new UserData() {Email = Email};
                    CurrentScreen=CurrentScreen.BasicData;
                    break;
                case CurrentScreen.BasicData:
                    if (!_validator.ValidateName(FirstName))
                    {
                        Error = "Należy podać imię";
                        return;
                    }
                    if (!_validator.ValidateName(LastName))
                    {
                        Error = "Należy podać nazwisko";
                        return;
                    }
                    _userData.FirstName = FirstName;
                    _userData.LastName = LastName;
                    _userData.CompanyName = CompanyName ?? string.Empty;
                    CurrentScreen=CurrentScreen.Address;
                    break;
            }
        }

        /// <summary>
        /// Asynchronicznie dokonuje rejestracji użytkownika w bazie danych
        /// </summary>
        /// <returns></returns>
        public async virtual Task RegisterAsync()
        {
            _address = new Address() {City = City, Street = Street, PostalCode = PostalCode, LocalNumber = LocaleNumber};
            try
            {
                await _service.SaveUserAsync(_user, _userData, _address);
                MessageBox.Show("Dodano nowego użytkownika", "Nowy użytkownik");
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd przy dodawaniu uzytkownika");
            }
            Close();
        }

        #region Properties
        /// <summary>
        /// Pobiera lub ustawia nazwę użytkownika
        /// </summary>
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia email użytkownika
        /// </summary>
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia imię użytkownika
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia nazwisko użytkownika
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia nazwę firmy użytkownika
        /// </summary>
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia ulicę z adresu użytkownika
        /// </summary>
        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia numer lokalu z adresu użytkownika
        /// </summary>
        public string LocaleNumber
        {
            get { return _localeNumber; }
            set
            {
                _localeNumber = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia kod pocztowy miejscowości użytkownika
        /// </summary>
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia miasto uzytkownika
        /// </summary>
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia wiadomość o przyczynie błędnej walidacji
        /// </summary>
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
