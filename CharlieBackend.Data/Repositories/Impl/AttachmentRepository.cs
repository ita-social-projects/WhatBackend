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

        public async Task<List<AttachmentDto>> GetAttachmentList(long accountId, long? courseId, long? groupId, long? requestedStudentAccountId, 
                                                                DateTime? startDate, DateTime? finishDate)
        {
            return await _applicationContext.Attachments
                    .WhereIf(_applicationContext.Accounts.Find(accountId).Role == UserRole.Mentor, x => _applicationContext.Mentors
                        .FirstOrDefault(y => y.AccountId == accountId).MentorsOfStudentGroups
                        .Select(f => f.StudentGroup)
                        .SelectMany(f => f.StudentsOfStudentGroups)
                        .Select(f => f.Student.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(courseId != default && courseId != 0, x => _applicationContext.StudentsOfStudentGroups
                        .Where(y => y.StudentGroup.CourseId == courseId)
                        .Select(z => z.Student.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(groupId != default && groupId != 0, x => _applicationContext.StudentsOfStudentGroups
                        .Where(y => y.StudentGroupId == groupId)
                        .Select(y => y.Student.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(requestedStudentAccountId != default && requestedStudentAccountId != 0, x => _applicationContext.Students
                        .Where(y => y.AccountId == requestedStudentAccountId)
                        .Select(y => y.AccountId)
                        .Contains(x.CreatedByAccountId))
                    .WhereIf(startDate != default, x => x.CreatedOn >= startDate)
                    .WhereIf(finishDate != default, x => x.CreatedOn <= finishDate)
                    .Select(x => new AttachmentDto
                    {
                        Id = x.Id,
                        CreatedOn = x.CreatedOn,
                        CreatedByAccountId = x.CreatedByAccountId,
                        ContainerName = x.ContainerName,
                        FileName = x.FileName
                    })
                    .ToListAsync(); 
        }

        public Task<List<Attachment>> GetAttachmentsByIdsAsync(IList<long> attachmentIds)
        {
            return _applicationContext.Attachments
                    .Where(attachment => attachmentIds.Contains(attachment.Id))
                    .ToListAsync();
        }
    }
}
