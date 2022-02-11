using CharlieBackend.Business.Services.FileServices.ExportFileServices.Csv;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportCSVFilesService
{
    public class CsvStudentsGroupExport : CsvFileExport<IEnumerable<Student>>
    {
        private string _fileGroupName;

        public CsvStudentsGroupExport(string fileGroupName)
        {
            _fileGroupName = fileGroupName;
        }

        public void FillFile(IEnumerable<Student> students)
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
        }

        public override async Task FillFileAsync(IEnumerable<Student> data)
        {
            await Task.Run(() => FillFile(data));
        }

        public override string GetFileName()
        {
            return $"{_fileGroupName}.csv";
        }
    }
}

