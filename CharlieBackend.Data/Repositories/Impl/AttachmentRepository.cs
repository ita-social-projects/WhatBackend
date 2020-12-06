using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;


namespace CharlieBackend.Data.Repositories.Impl
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository 
    { 
        public AttachmentRepository(ApplicationContext applicationContext)
                    : base(applicationContext)
        {
        }

    }

}
