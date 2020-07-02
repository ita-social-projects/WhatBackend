using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MentorOfCourseRepository : Repository<MentorOfCourse>, IMentorOfCourseRepository
    {
        public MentorOfCourseRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
