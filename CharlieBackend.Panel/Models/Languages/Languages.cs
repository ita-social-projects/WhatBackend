using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Models.Languages
{
    public enum Language
    {
        [Description("en-US")]
        En,
        [Description("uk-UA")]
        Uk
    }

    public static class Languages
    {
        public static Language language = Language.En;
    }

    public static class LanguageExtensions
    {
        public static string ToDescriptionString(this Language val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
