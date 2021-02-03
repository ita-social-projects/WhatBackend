using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public interface IGroupXlsFileImporter
    {
        Task<Result<IEnumerable<ImportStudentGroupDto>>> ImportGroupsAsync(long coursId, IFormFile file);
    }
}
