using CharlieBackend.Core.DTO.Export;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public class ImportOperatorProvider : IOperatorImportProvider
    {
        IServiceProvider _serviceProvider;
        private readonly IDictionary<FileExtension, Type> _importServiceTypes;

        public ImportOperatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _importServiceTypes = new Dictionary<FileExtension, Type>();

            _importServiceTypes.Add(FileExtension.XLSX, typeof(ServiceOperatorXlsx));
            _importServiceTypes.Add(FileExtension.CSV, typeof(ServiceOperatorCsv));
        }

        public IOperatorImport GetExportService(FileExtension extension)
        {
            if (_importServiceTypes.ContainsKey(extension))
            {
                return _serviceProvider.GetService(_importServiceTypes[extension]) as IOperatorImport;
            }

            return null;
        }

        public bool IsExtansionSupported(FileExtension extension) 
        {
            return _importServiceTypes.ContainsKey(extension);
        }
    }
}
