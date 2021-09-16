using CharlieBackend.Business.Services.FileServices.ExportFileServices.Csv;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportCSVFilesService
{
    public class StudentGroupCSVExporter : BaseFileExportCSV
    {
        private string _fileGroupName;

        public StudentGroupCSVExporter(string fileGroupName)
        {
            _fileGroupName = fileGroupName;
        }

        public Task FillFile(IList<Student> students)
        {
            StringBuilder line = new StringBuilder();

            foreach (var s in students)
            {
                line.Append(s.Account.FirstName + ';');
                line.Append(s.Account.LastName + ';');
                line.Append(s.Account.Email + ';' + '\n');
            }

            byte[] byteLine = ConvertLineToArray(line.ToString());

            _memoryStream.Write(byteLine);

            return Task.CompletedTask;
        }

        private byte[] ConvertLineToArray(string line)
        {
            byte[] array = new byte[line.Length];

            for (int i = 0; i < line.Length; i++)
            {
                array[i] = (byte)line[i];
            }

            return array;
        }

        public override string GetFileName()
        {
            return $"{_fileGroupName}.csv";
        }
    }
}

