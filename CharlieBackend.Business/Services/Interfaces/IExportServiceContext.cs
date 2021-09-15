using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Core.DTO.Export;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IExportServiceContext : IExportService
    {
        bool SetServise(FileExtension extension);
    }
}
