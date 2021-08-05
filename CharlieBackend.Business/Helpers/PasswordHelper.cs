using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Business.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// At least eight characters, at least one uppercase letter, one lowercase letter one number and special character
        /// </summary>
        private const string _pattern = @"^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$_%^&+=])(?=.*[0-9]).*$";
        private static readonly string _allowedSymbols = "qa2zWSXe4dc6RF8Vtg0bYHNujmIKolPpLOk7iMJUn9hy3BGTvf_rCDE5xs1wZAQ";
        private static readonly string _saltAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-01234567890";
        private static readonly int _saltLen = 15;

        public static string GenerateSalt()
        {
            //StringBuilder object with a predefined buffer size for the resulting string
            StringBuilder stringBuilder = new StringBuilder(_saltLen - 1);

            //a variable for storing a random character position from the string Str
            int Position = 0;

            for (int i = 0; i < _saltLen; i++)
            {
                Position = Next(0, _saltAlphabet.Length - 1);

                //add the selected character to the object StringBuilder
                stringBuilder.Append(_saltAlphabet[Position]);
            }

            return stringBuilder.ToString();
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] data = Encoding.Default.GetBytes(password + salt);
            var result = new SHA256Managed().ComputeHash(data);

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        private static Int32 Next(Int32 minValue, Int32 maxValue)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uint32Buffer = new byte[4];
                Int64 diff = maxValue - minValue;

                while (true)
                {
                    rng.GetBytes(uint32Buffer);
                    UInt32 rand = BitConverter.ToUInt32(uint32Buffer, 0);
                    Int64 max = (1 + (Int64)UInt32.MaxValue);
                    Int64 remainder = max % diff;

                    if (rand < max - remainder)
                    {
                        return (Int32)(minValue + (rand % diff));
                    }
                }
            }
        }

        public static bool PasswordValidation(string password)
        {
            if (password is null) 
                return false;
            return Regex.IsMatch(password, _pattern);
        }

        public static string GeneratePassword()
        {
            var validPassword = false;
            var password = new StringBuilder();
            var random = new Random();

            while (!validPassword)
            {
                _ = password.Clear();
                var passwordLength = ValidationConstants.MinLength;

                while (passwordLength-- > 0)
                {
                    _ = password.Append(_allowedSymbols[random.Next(_allowedSymbols.Length)]);
                }

                validPassword = Regex.IsMatch(password.ToString(), _pattern);
            }

            return password.ToString();
        }
    }
}
