using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.FileModels;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.ImportController
{
    public class ImportDataFromFileRequest : IExamplesProvider<ImportFileDto>
    {
        public ImportFileDto GetExamples()
        {
            return new ImportFileDto
            {
                url = "c:\\foo\filename.ext"
            };
        }
    }
}
