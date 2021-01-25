using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CharlieBackend.Business.Helpers
{
    public static class PasswordHelper
    {
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

        public static string PasswordValidation(string password)
        {
            char[] symbols = new[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', ',', '+', '=', '[', ']', '{', '}', ';', ':', '<', '>', '|', '.', '/', '?', ',', '-', '>', '<', '+', '=', '~' };

            if (string.IsNullOrWhiteSpace(password))
            {
                return "Password should not be empty";
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");



            if (!hasNumber.IsMatch(password))
            {
                return "Password should contain at least one numeric value";
            }

            else if (!hasUpperChar.IsMatch(password))
            {
                return "Password should contain at least one upper case letter";
            }
            else if (!hasLowerChar.IsMatch(password))
            {
                return "Password should contain at least one lower case letter";
            }
            else if (password.IndexOfAny(symbols) != -1)
            {
                return "Password should contain only this thing '_'";
            }
            else if (password.Length < 8)
            {
                return "Password length must be more than 8";
            }
            else
            {
                return null;
            }
        }
    }
}
