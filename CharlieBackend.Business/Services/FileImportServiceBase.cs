using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public abstract class FileImportServiceBase<T> 
    {
        public abstract Task<Result<List<T>>> ImportFileAsync(ImportFileDto file);

        protected abstract XLWorkbook GetFile(byte[] fileAsBytes);

        protected abstract bool IsEndOfFile(int rowCounter, IXLWorksheet ws);

        protected abstract Task IsValueValid(T fileLine, int rowCounter);
    }
}
