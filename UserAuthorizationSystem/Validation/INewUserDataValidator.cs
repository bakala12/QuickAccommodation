using System.Threading.Tasks;

namespace UserAuthorizationSystem.Validation
{
    public interface INewUserDataValidator
    {
        LoginValidationStatus ValidateUserLogin(string login);
        PasswordValidationStatus ValidateUserPassword(string password);
        PasswordValidationStatus ValidateUserPasswordConfirmed(string password, string password2);
        bool ValidateUserEmail(string email);
        Task<LoginValidationStatus> ValidateUserLoginAsync(string login);
    }
}
