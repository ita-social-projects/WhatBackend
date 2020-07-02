using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MentorRepository: Repository<Mentor>, IMentorRepository
    {
        public MentorRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
