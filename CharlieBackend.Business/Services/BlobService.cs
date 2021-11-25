using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class BlobService : IBlobService
    {
        private readonly AzureStorageBlobAccount _blobAccount;
        private readonly ILogger<BlobService> _logger;

        public BlobService(
                        AzureStorageBlobAccount blobAccount,
                        ILogger<BlobService> logger
                           )
        {
            _blobAccount = blobAccount;
            _logger = logger;
        }

        public string GetUrl(Attachment attachment)
        {
            BlobClient blob = new BlobClient
                       (
                       _blobAccount.ConnectionString,
                       attachment.ContainerName,
                       attachment.FileName
                       );

            return blob.Uri.AbsoluteUri;
        }

        public async Task<BlobClient> UploadAsync(string fileName, Stream fileStream, bool isPublic = false)
        {
            string containerName = Guid.NewGuid().ToString("N");

            BlobContainerClient container =
                        new BlobContainerClient(_blobAccount.ConnectionString, containerName);

            await container.CreateIfNotExistsAsync();

            if(isPublic)
                container.SetAccessPolicy(PublicAccessType.BlobContainer);

            BlobClient blob = container.GetBlobClient(fileName);

            _logger.LogInformation("FileName: " + fileName);
            _logger.LogInformation("Uri: " + blob.Uri);

            await blob.UploadAsync(fileStream);

            return blob;
        }

        public async Task<BlobDownloadInfo> DownloadAsync(string containerName, string fileName)
        {
            BlobClient blob = new BlobClient
                       (
                       _blobAccount.ConnectionString,
                       containerName,
                       fileName
                       );

            BlobDownloadInfo download = await blob.DownloadAsync();

            return download;
        }

        public async Task<bool> DeleteAsync(string containerName)
        {
            BlobContainerClient container = new BlobContainerClient
                        (
                        _blobAccount.ConnectionString,
                        containerName
                        );

            var response = await container.DeleteIfExistsAsync();

            return response;
        }
    }
}
