using CharlieBackend.Core.Entities;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Extensions
{
    public static class UserRoleMaster
    {
        public static bool Is(this UserRole currentRole, UserRole checkingRole) 
        {
            bool result = false;

            if (currentRole.HasFlag(checkingRole))
            {
                result = true;
            }

            return result;
        }

        public static bool IsNotAssigned(this UserRole currentRole) 
        {
            bool result = false;

            if (currentRole == UserRole.NotAssigned)
            {
                result = true;
            }

            return result;
        }

        public static bool IsAdmin(this UserRole currentRole)
        {
            bool result = false;

            if (currentRole == UserRole.Admin)
            {
                result = true;
            }

            return result;
        }

        private static bool CheckInputRole(UserRole role) 
        {
            bool result = true;

            if ((role != UserRole.Student) 
                    && (role != UserRole.Mentor) 
                    && (role != UserRole.Secretary))
            {
                result = false;
            }

            return result;
        }

        public static bool GrantAccountRole(this Account person, UserRole role)
        {
            bool result = true;

            if (person == null || person.Role.IsAdmin()
                       || person.Role.IsNotAssigned())
            {
                result = false;
            }
            else
            {
                if (!CheckInputRole(role))
                {
                    result = false;
                }
                else
                {
                    if (!person.Role.HasFlag(role))
                    {
                        person.Role |= role;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        public static async Task<bool> GrantAccountRoleAsync(this Account person,
                UserRole role)
        {
            return await Task.Run(() => person.GrantAccountRole(role));
        }

        public static bool RevokeAccountRole(this Account person,
                UserRole role)
        {
            bool result = true;

            if (person == null)
            {
                result = false;
            }

            if (CheckInputRole(role) && person.Role.HasFlag(role))
            {
                if ((person.Role &~ role) != UserRole.NotAssigned)
                {
                    person.Role &= ~role;
                }
                else
                {
                    result = false;
                }              
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static async Task<bool> RevokeAccountRoleAsync(
                this Account person, UserRole role)
        {
            return await Task.Run(() => person.RevokeAccountRole(role));
        }
    }
}
