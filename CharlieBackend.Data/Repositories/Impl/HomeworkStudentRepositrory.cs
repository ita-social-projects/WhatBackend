using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class HomeworkStudentRepositrory : Repository<HomeworkStudent>, IHomeworkStudentRepository
    {
        public HomeworkStudentRepositrory(ApplicationContext context)
            :base (context) 
        {

        }

        public async Task<IList<HomeworkStudent>> GetHomeworkStudentForStudentByStudentId(long id)
        {
            return await _applicationContext.HomeworkStudents
                .Where(x => x.StudentId == id)
                .ToListAsync();
        }

        public async Task<IList<HomeworkStudent>> GetHomeworkStudentForMentorByHomeworkId(long homeworkId)
        {
           return await _applicationContext.HomeworkStudents
                .Where(x => x.HomeworkId == homeworkId)
                .ToListAsync();



        }
    }
}
