﻿using System.IO;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class BaseFileExportHTML : FileExport
    {
        public BaseFileExportHTML()
        {
            _memoryStream = new MemoryStream();
        }
    }
}
