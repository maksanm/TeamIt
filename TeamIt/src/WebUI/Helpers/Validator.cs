using Blazorise;

namespace WebUI.Helpers
{
    public static class Validator
    {
        public static void IsPassword(ValidatorEventArgs e)
        {
            IsNotNullOrWhiteSpace(e, "Password");
            if (e.Status == ValidationStatus.Error)
                return;
            string password = (string)e.Value;
            if (!(password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsNumber) && password.Any(c => !char.IsLetterOrDigit(c))))
                SetError(e, "Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter and one special character");
        }

        public static void IsNotNullOrWhiteSpace(ValidatorEventArgs e, string fieldName)
        {
            string? value = (string?)e.Value;
            e.Status = ValidationStatus.Success;
            if (string.IsNullOrWhiteSpace(value))
                SetError(e, fieldName + " is required");
        }

        public static void IsEqualTo(ValidatorEventArgs e, string fieldName, string correctFieldName, string correctValue)
        {
            IsNotNullOrWhiteSpace(e, fieldName);
            if (e.Status == ValidationStatus.Error)
                return;
            string value = (string)e.Value;
            if (value != correctValue)
                SetError(e, fieldName + " is not equal to " + correctFieldName);
        }

        private static void SetError(ValidatorEventArgs e, string errorMessage)
        {
            e.Status = ValidationStatus.Error;
            e.ErrorText = errorMessage;
        }

        public static void IsNotEmpty(ValidatorEventArgs e, string fieldName)
        {
            string? value = GetValue(e);
            e.Status = ValidationStatus.Success;
            if (string.IsNullOrWhiteSpace(value))
            {
                SetError(e, fieldName + " is required");
            }
        }

        private static string? GetValue(ValidatorEventArgs e) => (string?)e.Value;
    }
}
