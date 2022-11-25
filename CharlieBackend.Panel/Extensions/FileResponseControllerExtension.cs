using CharlieBackend.Panel.Models.Export;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.Panel.Extensions
{
    /// <summary>
    /// Controller related extensions methods
    /// </summary>
    public static class FileResponseControllerExtension
    {
        private const string _csvContentType = "text/csv";
        private const string _htmlContentType = "text/html";
        private const string _excelContentType = "application/vnd.ms-excel";
        private const string _csvFileFormat = "csv";
        private const string _htmlFileFormat = "html";
        private const string _xlsxFileFormat = "xlsx";
        private const string _filenameFormat = "export.{0}";

        /// <summary>
        /// Creates File response. File download name and content type are related to file format.
        /// </summary>
        /// <param name="controller">MVC Controller</param>
        /// <param name="fileBytes">File bytes</param>
        /// <param name="format">File original format</param>
        /// <returns><see cref="FileContentResult"/> via <see cref="ControllerBase.File(byte[], string, string)"/></returns>
        public static IActionResult ExportFile(this Controller controller, byte[] fileBytes, ExportFileFormat format)
        {
            string contentType = string.Empty;
            string filename = string.Empty;

            switch(format)
            {
                case ExportFileFormat.Csv:
                    contentType = _csvContentType;
                    filename = string.Format(_filenameFormat, _csvFileFormat);
                    break;
                case ExportFileFormat.Html:
                    contentType = _htmlContentType;
                    filename = string.Format(_filenameFormat, _htmlFileFormat);
                    break;
                case ExportFileFormat.Xlsx:
                    contentType = _excelContentType;
                    filename = string.Format(_filenameFormat, _xlsxFileFormat);
                    break;
            }

            return controller.File(fileBytes, contentType, filename);
        }
    }
}
