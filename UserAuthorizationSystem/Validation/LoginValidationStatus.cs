using System;

namespace UserAuthorizationSystem.Validation
{
    public enum LoginValidationStatusEnumeration
    {
        Correct,
        TooShort,
        AlreadyUsed
    }

    public class LoginValidationStatus
    {
        public LoginValidationStatusEnumeration Status { get; }
        public string AdditionalInfo { get; }

        public LoginValidationStatus(LoginValidationStatusEnumeration status, string additionalInfo)
        {
            Status = status;
            AdditionalInfo = additionalInfo;
        }
    }
}
