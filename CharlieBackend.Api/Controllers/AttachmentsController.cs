using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manupulate with attachments
    /// </summary>
    [Route("api/attachments")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {

        private readonly IAttachmentService _attachmentService;

        /// <summary>
        /// Attachments controller constructor
        /// </summary>
        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        /// <summary>
        /// POST api/attachments
        /// </summary>
        /// <response code="200">Successful attachment</response>
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpPost]
        public async Task<IActionResult> PostAttachments([FromForm] IFormFileCollection fileCollection)
        {
            var userContext = HttpContext.User;

            var addedAttachments = await _attachmentService.AddAttachmentsAsync(fileCollection, userContext);

            return addedAttachments.ToActionResult();
        }

        /// <summary>
        /// GET: api/attachments
        /// </summary>
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpGet]
        public async Task<IActionResult> GetAttachments()
        {
            var attachments = await _attachmentService.GetAttachmentsListAsync();

            return attachments.ToActionResult();
        }

        /// <summary>
        /// GET: api/attachments/{id}
        /// </summary>
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpGet("{attachmentId}")]
        public async Task<IActionResult> GetAttachment(long attachmentId)
        {
            var attachment = await _attachmentService.DownloadAttachmentAsync(attachmentId);

            if (attachment.Data == null)
            {
                return attachment.ToActionResult();
            }

            return File
                (
                attachment.Data.DownloadInfo.Content, 
                attachment.Data.DownloadInfo.ContentType, 
                attachment.Data.FileName 
                );
        }

        /// <summary>
        /// DELETE: api/attachments/{id}
        /// </summary>
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(long attachmentId)
        {
            var attachment = await _attachmentService.DeleteAttachmentAsync(attachmentId);

            return attachment.ToActionResult();
        }
    }
}
