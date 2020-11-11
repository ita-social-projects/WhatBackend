using CharlieBackend.AdminPanel.Models.StudentGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IStudentGroupService
    {
        public Task<IList<StudentGroupViewModel>> GetAllStudentGroups(string accessToken);
    }
}
