using System.IO;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Csv
{
    public abstract class CsvFileExport<T> : FileExport<T> where T : class
    {
        protected virtual byte[] ConvertLineToArray(string line)
        {
            byte[] array = new byte[line.Length];

            for (int i = 0; i < line.Length; i++)
            {
                array[i] = (byte)line[i];
            }

            return array;
        }
    }
}
