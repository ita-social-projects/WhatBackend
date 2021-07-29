using CharlieBackend.Core.Entities;
using System;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Extensions
{
    public static class UserRoleExtension
    {
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

        public static Account SetAccountRole(this Account person, UserRole role)
        {
            if (person == null)
            {
                throw new NullReferenceException("Argument person equal null");
            }

            if (!CheckInputRole(role))
            {
                throw new ArgumentException("Role is unsuitable. You can set " +
                    "only less than Admin and more than NotAssigned and " +
                    "only one role");
            }

            if (!person.Role.HasFlag(role))
            {
                person.Role |= role;
            }
            else
            {
                throw new ArgumentException("Account allready has this role" +
                        " or role is unsuitable");
            }

            return person;
        }

        public static async Task<Account> SetAccountRoleAsync(this Account person, UserRole role)
        {
            return await Task.Run(() => person.SetAccountRole(role));
        }

        public static Account RemoveAccountRole(this Account person,
                UserRole role)
        {
            if (person == null)
            {
                throw new NullReferenceException("Argument person equal null");
            }

            if (CheckInputRole(role) && person.Role.HasFlag(role))
            {
                person.Role &= ~role;
            }
            else
            {
                throw new ArgumentException("Role is unsuitable. You can " +
                    "remove only less than Admin and more than NotAssigned");
            }

            return person;
        }

        public static async Task<Account> RemoveAccountRoleAsync(
                this Account person, UserRole role)
        {
            return await Task.Run(() => person.RemoveAccountRole(role));
        }
    }
}
