using CharlieBackend.Panel.Models.Export;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.Panel.Extensions
{
    public static class FileResponseControllerExtension
    {
        public static IActionResult ExportFile(this Controller controller, byte[] fileBytes, ExportFileFormat format)
        {
            string contentType = string.Empty;
            string filename = "export";

            switch(format)
            {
                case ExportFileFormat.Csv:
                    contentType = "text/csv";
                    filename += ".csv";
                    break;
                case ExportFileFormat.Html:
                    contentType = "text/html";
                    filename += ".html";
                    break;
                case ExportFileFormat.Xlsx:
                    contentType = "application/vnd.ms-excel";
                    filename += ".xlsx";
                    break;
            }

            return controller.File(fileBytes, contentType, filename);
        }
    }
}
