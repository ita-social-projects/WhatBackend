using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class ExportServiceContext : IExportServiceContext
    {
        IExportService _currentService;
        IUnitOfWork _unitOfWork;

        public ExportServiceContext(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsServiseSet 
        {
            get => _currentService != null;
        }

        public Task<Result<FileDto>> GetStudentClassbook(StudentsClassbookResultDto data)
        {
            return _currentService.GetStudentClassbook(data);
        }

        public Task<Result<FileDto>> GetStudentsClassbook(StudentsClassbookResultDto data)
        {
            return _currentService.GetStudentsClassbook(data);
        }

        public Task<Result<FileDto>> GetStudentGroupResults(StudentGroupsResultsDto data)
        {
            return _currentService.GetStudentGroupResults(data);
        }

        public Task<Result<FileDto>> GetStudentResults(StudentsResultsDto data)
        {
            return _currentService.GetStudentResults(data);
        }

        public Task<Result<FileDto>> GetStudentsResults(StudentsResultsDto data)
        {
            return _currentService.GetStudentsResults(data);
        }

        public Task<Result<FileDto>> GetListofStudentsByGroupId(long groupId)
        {
            return _currentService.GetListofStudentsByGroupId(groupId);
        }

        public bool SetServise(FileExtension extension)
        {
            bool result = true;

            switch (extension)
            {
                case FileExtension.XLSX:
                    _currentService = new ExportServiceXlsx();
                    break;

                case FileExtension.CSV:
                    _currentService = new ExportServiceCsv(_unitOfWork);
                    break;

                case FileExtension.Undefined:
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
