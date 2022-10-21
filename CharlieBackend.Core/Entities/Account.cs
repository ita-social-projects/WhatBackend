using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Account : BaseEntity
    {
        public UserRole Role { get; set; } = UserRole.NotAssigned;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Localization { get; set; }

#nullable enable
        public bool? IsActive { get; set; }

        public string? ForgotPasswordToken { get; set; }

        public DateTime? ForgotTokenGenDate { get; set; }

        public string? TelegramId { get; set; }

        public string? TelegramToken { get; set; }

        public DateTime? TelegramTokenGenDate { get; set; }

        //[ForeignKey("Avatar")]
        public long? AvatarId { get; set; }

#nullable disable

        public virtual Attachment Avatar { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }

        public virtual ICollection<Mentor> Mentors { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Secretary> Secretaries { get; set; }
    }
}
