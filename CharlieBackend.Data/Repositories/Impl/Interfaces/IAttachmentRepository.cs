using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Attachment;


namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        Task<List<AttachmentDto>> GetAttachmentList(long accountId, long? courseId, long? groupId, long? requestedAccId,
                                                                DateTime? startDate, DateTime? finishDate);
        Task<List<Attachment>> GetAttachmentsByIdsAsync(IList<long> attachmentIds);
    }
}
