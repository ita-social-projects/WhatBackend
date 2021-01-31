using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class FilterLessonsRequestDto    
    {
        public long? StudentGroupId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FinishDate { get; set; }
    }
}
