using CharlieBackend.Panel.Models.Export;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IExportService
    {
        public Task<byte[]> ExportStudentsClassbook(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel);
        public Task<byte[]> ExportStudentsResults(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel);
        public Task<byte[]> ExportStudentClassbook(ExportFileFormat format, long studentId, ExportByDateRangeModel exportModel);
        public Task<byte[]> ExportStudentResults(ExportFileFormat format, long studentId, ExportByDateRangeModel exportModel);
        public Task<byte[]> ExportStudentGroupResults(ExportFileFormat format, long courseId, ExportByDateRangeModel exportModel);
        public Task<byte[]> ExportStudentsOfGroup(ExportFileFormat format, long groupId);
    }
}
