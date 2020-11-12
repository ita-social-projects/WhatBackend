using CharlieBackend.Core.DTO.StudentGroups;
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
    public class DashboardRepository : Repository<StudentGroup>, IDashboardRepository
    {
        public DashboardRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {
        }
    }
}
