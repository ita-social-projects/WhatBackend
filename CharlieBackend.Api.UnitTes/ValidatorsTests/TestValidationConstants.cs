using System.Collections.Generic;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public static class TestValidationConstants
    {
        #region Email

        public const string ValidEmail = "ValidEmail@gmail.com";
        public const string NotValidEmail = "@ValidEmailgmail.com";

        #endregion

        #region Password

        public const string TooShortPassword = "VP_12";
        public const string TooLongPassword = "VeryValidAndEvenMoreEasyToRememberPassword12";
        public const string NoSpecialSymbolsPassword = "validPassword12";
        public const string NoNumbersPassword = "validPassword_";
        public const string NoUpperCasePassword = "validpassword12";
        public const string ValidPassword = "validPassword_12";
        public const string NotValidPassword = "VP_12";
        public const string NotValidConfirmPassword = "notEqualPassword";

        #endregion

        #region Name

        public const string ValidName = "Validname";
        public const string NotValidName = "TooLoooooooooooooooooooooooooooooooongname";
        public const string ValidFirstName = "Validfirstname";
        public const string ValidLastName = "Validlastname";
        public const string NotValidFirstName = "TooLooooooooooooooooooooongName";
        public const string NotValidLastName = "TooLooooooooooooooooooooongName";

        #endregion

        #region URL

        public const string ValidFormURL = "http://example.org/foo/bar.html";
        public const string NotValidFormURL = "http:\\example.org/foo/bar.html";

        #endregion

        #region IDs

        public static List<long> GetValidIDs()
        {
            return new List<long> { 1, 21, 30, 42, 54, 73 };
        }

        public static List<long> GetNotValidIDs()
        {
            return new List<long> { 0, 21, 30, 42, 54, 70 };
        }

        #endregion
    }
}
