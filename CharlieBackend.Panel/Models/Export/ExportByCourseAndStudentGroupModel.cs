namespace CharlieBackend.Panel.Models.Export
{
    public class ExportByCourseAndStudentGroupModel : ExportByDateRangeModel
    {
        public int CourseId { get; set; }

        public int StudentGroupId { get; set; }
    }
}
