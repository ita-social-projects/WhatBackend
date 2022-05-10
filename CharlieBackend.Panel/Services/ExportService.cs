using AutoMapper;
using CharlieBackend.Panel.Models.Export;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class ExportService : IExportService
    {
        private readonly IBinaryResultApiUtil _apiBinaryResultUtil;
        private readonly ExportApiEndpoints _exportApiEndpoints;

        public ExportService(
            IBinaryResultApiUtil apiBinaryResultUtil,
            IOptions<ApplicationSettings> options)
        {
            _apiBinaryResultUtil = apiBinaryResultUtil;
            _exportApiEndpoints = options.Value.Urls.ApiEndpoints.Export;
        }

        public Task<byte[]> GetExportedStudentsClassbooksAsync(ExportFileFormat format)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte[]> GetExportedStudentClassbookAsync(ExportFileFormat format)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte[]> GetExportedStudentGroupsResultsstudentGroupResultsAsync(ExportFileFormat format)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte[]> GetExportedStudentResultsAsync(ExportFileFormat format)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte[]> GetExportedStudentsOfGroupstudentsOfGroupAsync(ExportFileFormat format)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte[]> GetExportedStudentsResultsAsync(ExportFileFormat format
            , ExportByCourseAndStudentGroupModel exportModel)
        {
            var url = string.Format(_exportApiEndpoints.GetExportedStudentsResultsEndpoint, (int)format);

            return _apiBinaryResultUtil.PostAsync(url, exportModel);
        }
    }
}
