using CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportReaders;
using CharlieBackend.Core.DTO.Export;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public class FileReaderProvider : IFileReaderProvider
    {
        IServiceProvider _serviceProvider;
        private readonly IDictionary<FileExtension, Type> _importServiceTypes;

        public FileReaderProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _importServiceTypes = new Dictionary<FileExtension, Type>();

            _importServiceTypes.Add(FileExtension.XLSX, typeof(XlsxFileReader));
            _importServiceTypes.Add(FileExtension.CSV, typeof(CsvFileReader));
        }

        public IFileReader GetFileReader(FileExtension extension)
        {
            if (_importServiceTypes.ContainsKey(extension))
            {
                return _serviceProvider.GetService(_importServiceTypes[extension]) as IFileReader;
            }

            return null;
        }

        public bool IsExtansionSupported(FileExtension extension)
        {
            return _importServiceTypes.ContainsKey(extension);
        }
    }
}

