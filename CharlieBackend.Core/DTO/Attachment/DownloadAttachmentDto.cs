using Azure.Storage.Blobs.Models;


namespace CharlieBackend.Core.DTO.Attachment
{
    public class DownloadAttachmentDto
    {
        public BlobDownloadInfo downloadInfo { get; set; }

        public string fileName { get; set; }
    }
}
