using System.Collections.Generic;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        Task<List<Attachment>> GetAttachmentsByIdsAsync(IList<long> attachmentIds);
    }
}
