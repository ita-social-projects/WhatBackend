using CharlieBackend.Panel.Models.Export;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IExportService
    {
        Task<byte[]> ExportStudentsClassbook(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel);
        Task<byte[]> ExportStudentsResults(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel);
        Task<byte[]> ExportStudentClassbook(ExportFileFormat format, long studentId, ExportByDateRangeModel exportModel);
        Task<byte[]> ExportStudentResults(ExportFileFormat format, long studentId, ExportByDateRangeModel exportModel);
        Task<byte[]> ExportStudentGroupResults(ExportFileFormat format, long courseId, ExportByDateRangeModel exportModel);
        Task<byte[]> ExportStudentsOfGroup(ExportFileFormat format, long groupId);
    }
}
