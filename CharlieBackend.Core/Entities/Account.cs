using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Account : BaseEntity
    {
        public Account()
        {
            Mentors = new HashSet<Mentor>();
            Students = new HashSet<Student>();
            Secretaries = new HashSet<Secretary>();
        }

        public sbyte? Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Mentor> Mentors { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Secretary> Secretaries { get; set; }
    }
}
