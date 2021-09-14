using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MarkRepository : Repository<Mark>, IMarkRepository
    {
        public MarkRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {
        }
    }
}
