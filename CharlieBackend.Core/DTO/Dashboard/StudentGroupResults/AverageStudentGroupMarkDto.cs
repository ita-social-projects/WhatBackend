namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentGroupMarkDto
    {
        public string Course { get; set; }

        public string StudentGroup { get; set; }

        public decimal? AverageMark { get; set; }
    }
}
