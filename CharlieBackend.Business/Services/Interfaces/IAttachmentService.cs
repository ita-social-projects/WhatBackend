using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Azure.Storage.Blobs.Models;
using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
   public interface IAttachmentService
    {
        Task<Result<IList<AttachmentDto>>> AddAttachmentsAsync(IFormFileCollection fileCollection);

        Task<Result<IList<AttachmentDto>>> GetAttachmentsListAsync();

        Task<Result<DownloadAttachmentDto>> DownloadAttachmentAsync(long attachmentId);
    }
}
