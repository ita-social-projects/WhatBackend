namespace CharlieBackend.Panel.Models.Export
{
    public class ExportByCourseAndStudentGroupModel : ExportByDataRangeModel
    {
        public int CourseId { get; set; }

        public int StudentGroupId { get; set; }
    }
}
