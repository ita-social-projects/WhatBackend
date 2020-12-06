﻿using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        public Task<bool> IsCourseNameTakenAsync(string courseName);

        public Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds);

        Task<bool> DisableCourseByIdAsync(long id);
        Task<bool> IsCourseEmptyAsync(long id);
    }
}
