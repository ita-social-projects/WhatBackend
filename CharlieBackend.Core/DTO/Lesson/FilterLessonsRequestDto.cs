using System;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class FilterLessonsRequestDto 
    {
        public long? StudentGroupId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}