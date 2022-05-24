namespace CharlieBackend.Core.DTO.Schedule
{
    public class ContextForCreateScheduleDTO
    {
        public long GroupID { get; set; }

        public long? ThemeID { get; set; }

        public long? MentorID { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }
    }
}
