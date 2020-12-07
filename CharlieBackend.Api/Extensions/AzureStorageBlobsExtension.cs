using CharlieBackend.Core;
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
            var blobService = new AzureStorageBlobAccount(azureConnectionString);
            service.AddSingleton(blobService);
         }
    }
}
