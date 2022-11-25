using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MarkRepository : Repository<Mark>, IMarkRepository
    {
        public MarkRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {
        }

        public new async Task<Mark> GetByIdAsync(long id)
        {
            return await _applicationContext.Marks
                    .Include(mark => mark.Account)
                    .Include(mark => mark.HomeworkStudent)
                    .Include(mark => mark.HomeworkStudentHistory)
                    .FirstOrDefaultAsync(mark => mark.Id == id);
        }
    }
}
