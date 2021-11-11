using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Student
{
    public class UpdateStudentDto
    {
        #nullable enable

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public IList<long>? StudentGroupIds { get; set; }

        #nullable disable
    }
}
