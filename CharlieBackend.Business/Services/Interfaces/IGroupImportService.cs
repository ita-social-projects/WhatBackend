
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IGroupImportService
    {
        public Task<Result<List<StudentGroupFileModel>>> ImportFileAsync(IFormFile uploadedFile);

        public bool CheckIfExcelFile(IFormFile file);
    }
}
