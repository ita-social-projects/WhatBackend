using System;
namespace CharlieBackend.Business.Helpers
{
    public static class ValidationConstants
    {
        public const int _minLength = 8;
        public const int _maxLengthPassword = 30;
        public const int _maxLengthEmail = 50;
        public const int _maxLengthName = 30;
        public const int _maxLengthURL = 200;
        public const string _passwordRule = "Password must have at least eight characters, at least one uppercase letter, one lowercase letter and one number";
        public const string _passwordConfirmNotValid = "Passwords do not match";
    }
}
