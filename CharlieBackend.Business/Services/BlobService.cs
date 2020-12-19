using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public async Task<Result<BlobClient>> UploadAsync(string fileName, Stream fileStream)
        {
            string containerName = Guid.NewGuid().ToString("N");

            BlobContainerClient container =
                        new BlobContainerClient(_blobAccount.connectionString, containerName);

            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(fileName);

            _logger.LogInformation("FileName: " + fileName);
            _logger.LogInformation("Uri: " + blob.Uri);

            await blob.UploadAsync(fileStream);

            return Result<BlobClient>.GetSuccess(blob);
        }

        public async Task<Result<BlobDownloadInfo>> DownloadAsync(string containerName, string fileName)
        {
            BlobClient blob = new BlobClient
                       (
                       _blobAccount.connectionString,
                       containerName,
                       fileName
                       );

            BlobDownloadInfo download = await blob.DownloadAsync();

            return Result<BlobDownloadInfo>.GetSuccess(download);
        }

        public async Task<Result<bool>> DeleteAsync(string containerName)
        {
            BlobContainerClient container = new BlobContainerClient
                        (
                        _blobAccount.connectionString,
                        containerName
                        );

            var response = await container.DeleteIfExistsAsync();

            return Result<bool>.GetSuccess(response);
        }
    }
}
