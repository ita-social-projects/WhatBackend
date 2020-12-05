using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core;
using System.Threading;
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
            var addedAttachments = await _attachmentService.AddAttachmentsAsync(fileCollection);

            return addedAttachments.ToActionResult();
        }
         /*
        // GET: api/<AttachmentsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AttachmentsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        // PUT api/<AttachmentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AttachmentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
         */
    }
}
