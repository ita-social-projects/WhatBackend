using Azure.Storage.Blobs.Models;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class DownloadAttachmentDto
    {
        public BlobDownloadInfo DownloadInfo { get; set; }

        public string FileName { get; set; }
    }
}
