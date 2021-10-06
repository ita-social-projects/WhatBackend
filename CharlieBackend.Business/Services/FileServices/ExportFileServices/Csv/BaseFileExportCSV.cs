using System.IO;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Csv
{
    public class BaseFileExportCSV : BaseFileExport
    {
        public BaseFileExportCSV()
        {
            _memoryStream = new MemoryStream();
        }
    }
}
