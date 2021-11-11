namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentVisitsDto
    {
        public string Course { get; set; }

        public string StudentGroup { get; set; }

        public string Student { get; set; }

        public int StudentAverageVisitsPercentage { get; set; }
    }
}
