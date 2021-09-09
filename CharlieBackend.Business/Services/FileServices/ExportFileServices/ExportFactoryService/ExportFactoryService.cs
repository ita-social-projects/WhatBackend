using CharlieBackend.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ExportFactoryService : IExportFactoryService
    {
        public IExportService GetExportService(ExportFileExtension fileExtension)
        {
            return fileExtension switch
            {
                ExportFileExtension.xlsx => new ExportServiceXlsx(),
                _ => new ExportServiceXlsx()
            };
        }
    }
}
