using CharlieBackend.Core.DTO.Export;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ExportServiceProvider : IExportServiceProvider
    {
        IServiceProvider _serviceProvider;
        private readonly IDictionary<FileExtension, Type> _exportServiceTypes;

        public ExportServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _exportServiceTypes = new Dictionary<FileExtension, Type>();

            _exportServiceTypes.Add(FileExtension.XLSX, typeof(ExportServiceXlsx));
            _exportServiceTypes.Add(FileExtension.CSV, typeof(ExportServiceCsv));
            _exportServiceTypes.Add(FileExtension.HTML, typeof(ExportServiceHtml));
        }

        public IExportService GetExportService(FileExtension fileExtension)
        {
            if (_exportServiceTypes.ContainsKey(fileExtension))
            {
                return _serviceProvider.GetService(_exportServiceTypes[fileExtension]) as IExportService;
            }

            return null;
        }
    }
}
