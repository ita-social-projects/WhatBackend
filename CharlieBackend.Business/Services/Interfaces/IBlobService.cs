using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CharlieBackend.Core.Entities;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IBlobService
    {
        Task<BlobClient> UploadAsync(string fileName, Stream fileStream, bool isPublic = false);

        Task<BlobDownloadInfo> DownloadAsync(string containerName, string fileName);

        Task<bool> DeleteAsync(string containerName);

        public string GetUrl(Attachment attachment);
    }
}
