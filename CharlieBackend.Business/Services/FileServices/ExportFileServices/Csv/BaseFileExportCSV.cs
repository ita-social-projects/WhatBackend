using System.IO;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Csv
{
    public class BaseFileExportCSV : FileExport
    {
        public BaseFileExportCSV()
        {
            _memoryStream = new MemoryStream();
        }
    }
}
