using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;

namespace CharlieBackend.Api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AzureStorageBlobsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public static void AddAzureStorageBlobs(this IServiceCollection service, string azureConnectionString)
        {
            var blobService = new BlobServiceClient(azureConnectionString);
            service.AddSingleton(blobService);
        }
    }
}
