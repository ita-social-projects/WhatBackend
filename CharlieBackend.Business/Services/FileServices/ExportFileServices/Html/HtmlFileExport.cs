using System.IO;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class HtmlFileExport : FileExport
    {
        public HtmlFileExport()
        {
            _memoryStream = new MemoryStream();
        }
    }
}
