using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CharlieBackend.Core.Entities
{
    [Serializable]
    public partial class Account : BaseEntity, CharlieBackend.Core.Interfaces.ICloneable<Account>
    {
        public UserRole Role { get; set; } = UserRole.NotAssigned;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

#nullable enable
        public bool? IsActive { get; set; }

        public string? ForgotPasswordToken { get; set; }

        public DateTime? ForgotTokenGenDate { get; set; }

        //[ForeignKey("Avatar")]
        public long? AvatarId { get; set; }
#nullable disable

        public virtual Attachment Avatar { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }

        public virtual ICollection<Mentor> Mentors { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Secretary> Secretaries { get; set; }

        public Account Clone()
        {
            Account account = null;
            using (MemoryStream tempStream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter(null,
                    new StreamingContext(StreamingContextStates.Clone));

                try
                {
                    binFormatter.Serialize(tempStream, this);
                    tempStream.Seek(0, SeekOrigin.Begin);

                    account = binFormatter.Deserialize(tempStream) as Account;
                }
                catch(Exception ex)
                { }
            }
            return account;
        }
    }
}
