using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class BaseFileExportHTML : BaseFileExport
    {
        public BaseFileExportHTML()
        {
            _memoryStream = new MemoryStream();
        }
    }
}
