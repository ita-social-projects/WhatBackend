using CharlieBackend.Core.Entities;

namespace CharlieBackend.Core.DTO.HomeworkStudent
{
    public class UpdateMarkRequestDto
    {
        public long StudentHomeworkId { get; set; }
        public int StudentMark { get; set; }
        public string MentorComment { get; set; }
        public MarkType MarkType { get; set; }
    }
}
