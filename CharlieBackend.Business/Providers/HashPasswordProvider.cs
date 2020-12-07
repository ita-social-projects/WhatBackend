using System;
using System.Text;
using System.Security.Cryptography;

namespace CharlieBackend.Business.Providers
{
    public static class HashPasswordProvider
    {
        private static readonly string _saltAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-01234567890";
        private static readonly int _saltLen = 15;

        public static string GenerateSalt()
        {
            //StringBuilder object with a predefined buffer size for the resulting string
            StringBuilder sb = new StringBuilder(_saltLen - 1);

            //a variable for storing a random character position from the string Str
            int Position = 0;

            for (int i = 0; i < _saltLen; i++)
            {
                Position = Next(0, _saltAlphabet.Length - 1);

                //add the selected character to the object StringBuilder
                sb.Append(_saltAlphabet[Position]);
            }

            return sb.ToString();
        }
        public static Int32 Next(Int32 minValue, Int32 maxValue)
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

        public static string HashPassword(string password, string salt)
        {
            byte[] data = Encoding.Default.GetBytes(password + salt);
            var result = new SHA256Managed().ComputeHash(data);

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
    }
}
