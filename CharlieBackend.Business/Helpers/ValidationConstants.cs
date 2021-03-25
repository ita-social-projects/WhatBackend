using System;
namespace CharlieBackend.Business.Helpers
{
    public static class ValidationConstants
    {
        public const int MinLength = 8;
        public const int MaxLengthPassword = 30;
        public const int MaxLengthEmail = 50;
        public const int MaxLengthName = 30;
        public const int MaxLengthURL = 200;
        public const string PasswordRule = "Password must have at least eight characters, at least one uppercase letter, one lowercase letter and one number";
        public const string PasswordConfirmNotValid = "Passwords do not match";
        public const string DatesNotValid = "StartDate can not be greater than FinisDate";

    }
}
