using System.IO;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IBlobService
    {
        Task<BlobClient> UploadAsync(string fileName, Stream fileStream);

        Task<BlobDownloadInfo> DownloadAsync(string containerName, string fileName);

        Task<bool> DeleteAsync(string containerName);
    }
}
