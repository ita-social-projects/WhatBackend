using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        Task<List<Attachment>> GetAttachmentListFiltered(AttachmentRequestDto request);
        Task<List<Attachment>> GetAttachmentsByIdsAsync(IList<long> attachmentIds);
    }
}
