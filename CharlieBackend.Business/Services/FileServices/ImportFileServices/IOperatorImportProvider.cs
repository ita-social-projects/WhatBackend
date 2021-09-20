using CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportOperators;
using CharlieBackend.Core.DTO.Export;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public interface IOperatorImportProvider
    {
        public IOperatorImport GetExportService(FileExtension fileExtension);
    }
}
