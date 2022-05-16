using CharlieBackend.Panel.Models.Export;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.Panel.Extensions
{
    public static class FileResponseControllerExtension
    {
        private const string _csvContentType = "text/csv";
        private const string _htmlContentType = "text/html";
        private const string _excelContentType = "application/vnd.ms-excel";
        private const string _csvFileFormat = ".csv";
        private const string _htmlFileFormat = ".html";
        private const string _xlsxFileFormat = ".xlsx";

        public static IActionResult ExportFile(this Controller controller, byte[] fileBytes, ExportFileFormat format)
        {
            string contentType = string.Empty;
            string filename = "export";

            switch(format)
            {
                case ExportFileFormat.Csv:
                    contentType = _csvContentType;
                    filename += _csvFileFormat;
                    break;
                case ExportFileFormat.Html:
                    contentType = _htmlContentType;
                    filename += _htmlFileFormat;
                    break;
                case ExportFileFormat.Xlsx:
                    contentType = _excelContentType;
                    filename += _xlsxFileFormat;
                    break;
            }

            return controller.File(fileBytes, contentType, filename);
        }
    }
}
