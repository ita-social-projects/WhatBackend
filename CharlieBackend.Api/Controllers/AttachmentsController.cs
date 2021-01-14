using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Attachment;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
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
        /// Gets relevant attachments
        /// </summary>
        /// <param name="request"/>
        /// Returns list of attachments depending on the user role i.e admin and secretary can see all attachemnts, mentors only for the related groups and students only for themselves
        /// Mention course id/ group id/ student id or date range to sort the result (non required params)
        [SwaggerResponse(200, type: typeof(AttachmentDto))]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpGet]
        public async Task<IActionResult> GetAttachments([FromBody] AttachmentRequestDTO request)
        {
            var attachments = await _attachmentService.GetAttachmentsListAsync(request);

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
