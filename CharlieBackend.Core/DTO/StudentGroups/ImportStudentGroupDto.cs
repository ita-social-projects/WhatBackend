using System;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class ImportStudentGroupDto
    {
        public long Id { get; set; }

        public long? CourseId { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }
    }
}
