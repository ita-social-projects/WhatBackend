﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Attachment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manipulate attachments
    /// </summary>
    [Route("api/v{version:apiVersion}/attachments")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
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
            var addedAttachments = await _attachmentService.AddAttachmentsAsync(fileCollection);

            return addedAttachments.ToActionResult();
        }

        /// <summary>
        /// Adds avatar
        /// </summary>
        /// <param name="file">Files to add as attachment.</param>
        /// <response code="200">File is successfully attached.</response>
        /// <response code="HTTP: 400, API: 0">File exceed 50 MB or extension of file is prohibited.</response>
        [SwaggerResponse(200, type: typeof(IList<AttachmentDto>))]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpPost("avatar")]
        public async Task<IActionResult> PostAvatar(IFormFile file)
        {
            var addedAttachments = await _attachmentService.AddAttachmentAsAvatarAsync(file);

            return addedAttachments.ToActionResult();
        }

        /// <summary>
        /// Gets Avatar Url
        /// </summary>
        /// <remarks>
        /// Attachment of avatar is taken from Authenticated user account
        /// </remarks>
        [SwaggerResponse(200, type: typeof(string))]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpGet("avatar/url")]
        public async Task<ActionResult<string>> GetAvatarUrl()
        {
            var attachment = await _attachmentService.GetAvatarUrl();

            return attachment.ToActionResult();
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
            var attachments = await _attachmentService.GetAttachmentsListAsync(request);

            return attachments.ToActionResult();
        }

        /// <summary>
        /// Gets attachment by id
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
