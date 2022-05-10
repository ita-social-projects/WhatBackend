using CharlieBackend.Panel.Models.Export;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IExportService
    {
        public Task<byte[]> GetExportedStudentsClassbooksAsync(ExportFileFormat format);
        public Task<byte[]> GetExportedStudentsResultsAsync(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel);
        public Task<byte[]> GetExportedStudentClassbookAsync(ExportFileFormat format);
        public Task<byte[]> GetExportedStudentResultsAsync(ExportFileFormat format);
        public Task<byte[]> GetExportedStudentGroupsResultsstudentGroupResultsAsync(ExportFileFormat format);
        public Task<byte[]> GetExportedStudentsOfGroupstudentsOfGroupAsync(ExportFileFormat format);
    }
}
