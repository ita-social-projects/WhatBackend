using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Helpers
{
    public static class StringExtensions
    {
        public static byte[] ConvertLineToArray(this string line)
        {
            byte[] array = new byte[line.Length];

            for (int i = 0; i < line.Length; i++)
            {
                array[i] = (byte)line[i];
            }

            return array;
        }
    }
}
