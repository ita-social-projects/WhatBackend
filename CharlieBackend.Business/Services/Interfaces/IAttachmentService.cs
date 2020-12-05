using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Models.ResultModel;


namespace CharlieBackend.Business.Services.Interfaces
{
   public interface IAttachmentService
    {
        Task<Result<AttachmentDto>> AddAttachmentsAsync(IFormFileCollection fileCollection);
    }
}
