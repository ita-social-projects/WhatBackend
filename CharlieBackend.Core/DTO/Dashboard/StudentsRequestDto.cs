using System;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentsRequestDto<T> where T : Enum
    {
        public long? CourseId { get; set; }

        public long? StudentGroupId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public T[] IncludeAnalytics { get; set; }
    }
}
