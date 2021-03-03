using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Attachment;


namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        Task<List<Attachment>> GetAttachmentListFiltered(AttachmentRequestDto request);
        Task<List<Attachment>> GetAttachmentsByIdsAsync(IList<long> attachmentIds);
    }
}
