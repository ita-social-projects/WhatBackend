using CharlieBackend.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ExportServiceProvider : IExportServiceProvider
    {
        private readonly IDictionary<ExportFileExtension, IExportService> _exportServiceTypes;

        public ExportServiceProvider()
        {
            _exportServiceTypes = new Dictionary<ExportFileExtension, IExportService>();

            _exportServiceTypes.Add(ExportFileExtension.xlsx, new ExportServiceXlsx());
        }

        public IExportService GetExportService(ExportFileExtension fileExtension)
        {
            return _exportServiceTypes[fileExtension];
        }
    }
}
