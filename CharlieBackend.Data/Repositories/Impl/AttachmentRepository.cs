using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository 
    { 
        public AttachmentRepository(ApplicationContext applicationContext)
                    : base(applicationContext)
        {
        }

        public Task<List<Attachment>> GetAttachmentsByIdsAsync(IList<long> attachmentIds)
        {
            return _applicationContext.Attachments
                    .Where(attachment => attachmentIds.Contains(attachment.Id))
                    .ToListAsync();
        }
    }
}
