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

        public Task<byte[]> ExportStudentClassbook(ExportFileFormat format, long studentId, ExportByDateRangeModel exportModel)
        {
            var url = string.Format(_exportApiEndpoints.ExportStudentClassbookEndpoint
                , studentId, (int)format);

            return _apiBinaryResultUtil.PostAsync(url, exportModel);
        }

        public Task<byte[]> ExportStudentGroupResults(ExportFileFormat format, long courseId, ExportByDateRangeModel exportModel)
        {
            var url = string.Format(_exportApiEndpoints.ExportStudentGroupResultsEndpoint
                , courseId, (int)format);

            return _apiBinaryResultUtil.PostAsync(url, exportModel);
        }

        public Task<byte[]> ExportStudentResults(ExportFileFormat format, long studentId, ExportByDateRangeModel exportModel)
        {
            var url = string.Format(_exportApiEndpoints.ExportStudentResultsEndpoint
                , studentId, (int)format);

            return _apiBinaryResultUtil.PostAsync(url, exportModel);
        }

        public Task<byte[]> ExportStudentsClassbook(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel)
        {
            var url = string.Format(_exportApiEndpoints.ExportStudentsClassbookEndpoint, (int)format);

            return _apiBinaryResultUtil.PostAsync(url, exportModel);
        }

        public Task<byte[]> ExportStudentsOfGroup(ExportFileFormat format, long groupId)
        {
            var url = string.Format(_exportApiEndpoints.ExportStudentsOfGroupEndpoint, (int)format);

            return _apiBinaryResultUtil.GetAsync(url);
        }

        public Task<byte[]> ExportStudentsResults(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel)
        {
            var url = string.Format(_exportApiEndpoints.ExportStudentsResultsEndpoint, (int)format);

            return _apiBinaryResultUtil.PostAsync(url, exportModel);
        }
    }
}
