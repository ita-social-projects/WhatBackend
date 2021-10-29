using CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportReaders;
using CharlieBackend.Core.DTO.Export;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public interface IFileReaderProvider
    {
        public IFileReader GetFileReader(FileExtension fileExtension);
    }
}
