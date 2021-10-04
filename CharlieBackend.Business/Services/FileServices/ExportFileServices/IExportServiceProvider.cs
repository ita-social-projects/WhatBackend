using CharlieBackend.Core.DTO.Export;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public interface IExportServiceProvider
    {
        public IExportService GetExportService(FileExtension fileExtension);
    }
}
