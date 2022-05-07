using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.AccountDtoValidatorsTests
{
    public static class TestAccountValidationConstants
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

        #endregion

        #region URL

        public const string ValidFormURL = "http://example.org/foo/bar.html";
        public const string NotValidFormURL = "http:\\example.org/foo/bar.html";

        #endregion
    }
}
