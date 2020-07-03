using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        public Task<bool> IsCourseNameTakenAsync(string courseName);
    }
}
