namespace CharlieBackend.Core.Constants
{
    public static class HtmlFileExportConstants
    {
        public static string StudentsResultsFileHeaderConstant(string studentName) => $"Student: {studentName} average results";

        public static string StudentsResultsFileNameConstant(string date) => $"StudentResults_{date}.html";
    }
}
