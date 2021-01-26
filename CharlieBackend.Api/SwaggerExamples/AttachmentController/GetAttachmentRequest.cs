using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.AttachmentController
{
    internal class GetAttachmentRequest : IExamplesProvider<AttachmentRequestDto>
    {
        public AttachmentRequestDto GetExamples()
        {
            return new AttachmentRequestDto
            {
                CourseID = 0,
                GroupID = 0,
                StudentAccountID = 0,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now
            };
        }
    }
}
