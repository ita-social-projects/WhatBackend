using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Attachment;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Business.Services.Interfaces;
using System.Collections.Generic;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manipulate attachments
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
        /// Adds attachments
        /// </summary>
        /// <param name="fileCollection">Collection of files to add as attachments.</param>
        /// <response code="200">Files are successfully attached.</response>
        /// <response code="HTTP: 400, API: 0">Some of the files exceed 50 MB or some of the files' extensions are prohibited.</response>
        [SwaggerResponse(200, type: typeof(IList<AttachmentDto>))]
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
        /// <param name="request">Set of optional parameters to filter the attachments.</param>
        /// <remarks>
        /// Returns a list of attachments depending on the user role. So admin and secretary can see all attachemnts, mentors only for the related groups, and students only for themselves.
        /// Specify course id, group id, account id of a student, or date range to filter the result.
        /// </remarks>
        /// <response code="200">A list of attachments returned successfully.</response>
        /// <response code="HTTP: 400, API: 0">Invalid filters object.</response>
        [SwaggerResponse(200, type: typeof(IList<AttachmentDto>))]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpPost("attachments")]
        public async Task<IActionResult> GetAttachments([FromBody]AttachmentRequestDto request)
        {
            var userData = HttpContext.User;

            var attachments = await _attachmentService.GetAttachmentsListAsync(request, userData);

            return attachments.ToActionResult();
        }

        /// <summary>
        /// Downloads attachment
        /// </summary>
        /// <param name="attachmentId">Id of an attachment to download.</param>
        /// <response code="200">File is ready for download.</response>
        /// <response code="HTTP: 400, API: 0">Invalid attachment id.</response>
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
        /// Deletes attachment
        /// </summary>
        /// <param name="attachmentId">Id of an attachment to delete.</param>
        /// <response code="200">Info of a file that was successfully deleted.</response>
        /// <response code="HTTP: 400, API: 0">Invalid attachment id.</response>
        [SwaggerResponse(200, type: typeof(AttachmentDto))]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(long attachmentId)
        {
            var attachment = await _attachmentService.DeleteAttachmentAsync(attachmentId);

            return attachment.ToActionResult();
        }
    }
}
