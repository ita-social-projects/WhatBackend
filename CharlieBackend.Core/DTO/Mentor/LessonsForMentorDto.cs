using System;

namespace CharlieBackend.Core.DTO.Mentor
{
    public class LessonsForMentorDto
    {
        public long StudentGroupId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}
