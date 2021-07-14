using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

using CharlieBackend.Core.DTO.Attachment;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository 
    { 
        public AttachmentRepository(ApplicationContext applicationContext)
                    : base(applicationContext)
        {
        }

        public async Task<List<Attachment>> GetAttachmentListFiltered(AttachmentRequestDto request)
        {
            return await _applicationContext.Attachments
                    .WhereIf(request.MentorID.HasValue, x => _applicationContext.Mentors
                        .FirstOrDefault(y => y.Id == request.MentorID)
                        .MentorsOfStudentGroups
                        .Select(f => f.StudentGroup)
                        .SelectMany(f => f.StudentsOfStudentGroups)
                        .Select(f => f.Student.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(request.CourseID.HasValue, x => _applicationContext.StudentsOfStudentGroups
                        .Where(y => y.StudentGroup.CourseId == request.CourseID)
                        .Select(z => z.Student.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(request.GroupID.HasValue, x => _applicationContext.StudentsOfStudentGroups
                        .Where(y => y.StudentGroupId == request.GroupID)
                        .Select(y => y.Student.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(request.StudentAccountID.HasValue, x => _applicationContext.Students
                        .Where(y => y.Id == request.StudentAccountID)
                        .Select(y => y.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(request.StartDate.HasValue, x => x.CreatedOn >= request.StartDate)
                    .WhereIf(request.FinishDate.HasValue, x => x.CreatedOn <= request.FinishDate)
                    .ToListAsync(); 
        }

        public async Task<List<Attachment>> GetAttachmentsByIdsAsync(IList<long> attachmentIds)
        {
            return await _applicationContext.Attachments
                    .Where(attachment => attachmentIds.Contains(attachment.Id))
                    .ToListAsync();
        }
    }
}
