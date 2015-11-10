//using System;
//using System.ComponentModel;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Input;
//using AccommodationApplication.Commands;
//using AccommodationApplication.Converter;
//using AccommodationDataAccess.Domain;
//using AccommodationDataAccess.Model;
//using UserAuthorizationSystem.Registration;
//using UserAuthorizationSystem.Validation;

//namespace AccommodationApplication.ViewModels
//{
//    public class RegiserNewUserViewModel : CloseableViewModel
//    {
        
//        private readonly IUserCredentialsValidator _validator;
//        private readonly IRegisterUser _register;

//        public RegiserNewUserViewModel(IUserCredentialsValidator validator, IRegisterUser register)
//        {
//            if (validator == null || register == null) throw new ArgumentNullException();
//            _validator = validator;
//            _register = register;
//            RegisterCommand = new DelegateCommand(async x => await RegisterAsync(x as PasswordBox[]));
//        }

//        public ICommand RegisterCommand { get; }

//        public virtual async Task RegisterAsync(PasswordBox[] passwords)
//        {
//            if (passwords == null || passwords.Length != 2)
//                throw new InvalidOperationException();
//            //Validation
//            Error = string.Empty;
//            string reason;
//            if (string.IsNullOrEmpty(Username))
//            {
//                Error = "Login nie może być pusty";
//                return;
//            }
//            if (!_validator.ValidateEmail(Email))
//            {
//                Error = "Nieprawidłowy adres email";
//                return;
//            }
//            if (!_validator.ValidatePassword(passwords[0].Password, out reason))
//            {
//                Error = reason;
//                return;
//            }
//            if (!_validator.ValidatePasswordConfirmation(passwords[0].Password, passwords[1].Password))
//            {
//                Error = "Hasła nie są zgodne";
//                return;
//            }
//            bool b = await _validator.ValidateUsernameAsync<AccommodationContext>(Username);
//            if (!b)
//            {
//                Error = "Login jest już zajęty";
//                return;
//            }
//            // Imie nazwisko
//            if (string.IsNullOrEmpty(FirstName))
//            {
//                Error = "Należy podać imię";
//                return;
//            }
//            if (string.IsNullOrEmpty(LastName))
//            {
//                Error = "Należy podać nazwisko";
//                return;
//            }
//            //Register logic here
//            User user = _register.GetNewUser(Username, passwords[0].Password);
//            UserData userData = new UserData();
//            userData.FirstName = FirstName;
//            userData.LastName = LastName;
//            userData.Email = Email;
//            userData.CompanyName = CompanyName ?? string.Empty;
//            Address address = new Address();
//            //Address validation and setting...
//            await _register.SaveUserAsync<AccommodationContext>(user, userData, address);
//            Close();
//        }

        
//    }
//}
