using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Extensions
{
    public static class UserRolesExtension
    {
        public static Roles ToEnum<Roles>(this string value)
        {
            return (Roles)Enum.Parse(typeof(Roles), value);
        }
    }
}
