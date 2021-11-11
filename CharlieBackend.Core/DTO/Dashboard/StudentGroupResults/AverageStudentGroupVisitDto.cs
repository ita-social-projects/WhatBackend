namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentGroupVisitDto
    {
        public string Course { get; set; }

        public string StudentGroup { get; set; }

        public long AverageVisitPercentage { get; set; }
    }
}
