namespace UserAuthorizationSystem.Validation
{
    public enum PasswordValidationStatusEnumeration
    {
        Correct,
        TooShort,
        TooWeak,
        ConfirmationFailed
    }

    public class PasswordValidationStatus
    {
        public PasswordValidationStatusEnumeration Status { get; }
        public string AdditionalInfo { get; }

        public PasswordValidationStatus(PasswordValidationStatusEnumeration status, string additionalInfo)
        {
            Status = status;
            AdditionalInfo = additionalInfo;
        }
    }
}
