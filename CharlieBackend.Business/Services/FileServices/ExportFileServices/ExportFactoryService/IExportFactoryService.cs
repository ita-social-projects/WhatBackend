using CharlieBackend.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public interface IExportFactoryService
    {
        public IExportService GetExportService(ExportFileExtension fileExtension);
    }
}
