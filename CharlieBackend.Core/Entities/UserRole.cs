using System;

namespace CharlieBackend.Core.Entities
{
    [Flags]
    public enum UserRole : byte
    {
        NotAssigned = 0,
        Student = 1,
        Mentor = 2,
        Admin = 4,
        Secretary = 8
    }
}
