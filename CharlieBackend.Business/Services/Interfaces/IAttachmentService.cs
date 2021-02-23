using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Models.ResultModel;


namespace CharlieBackend.Business.Services.Interfaces
{
   public interface IAttachmentService
    {
        Task<Result<IList<AttachmentDto>>> AddAttachmentsAsync(IFormFileCollection fileCollection, bool isPublic = false);

        Task<Result<AttachmentDto>> AddAttachmentAsync(IFormFile file, bool isPublic = false);

        Task<Result<AttachmentDto>> AddAttachmentAsAvatarAsync(IFormFile file);

        Task<string> GetAvatarUrl();

        Task<Result<IList<AttachmentDto>>> GetAttachmentsListAsync(AttachmentRequestDto request);

        Task<Result<DownloadAttachmentDto>> DownloadAttachmentAsync(long attachmentId);

        Task<Result<AttachmentDto>> DeleteAttachmentAsync(long attachmentId);

        Task<string> GetAttachmentUrl(long id);
    }
}
