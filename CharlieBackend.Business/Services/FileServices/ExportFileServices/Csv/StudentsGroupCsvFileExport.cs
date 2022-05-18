using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportCSVFilesService
{
    class StudentsGroupCsvFileExport : FileExport<IEnumerable<Student>>
    {
        private string _fileGroupName;

        public StudentsGroupCsvFileExport(string fileGroupName)
        {
            _fileGroupName = fileGroupName;
        }

        public override async Task FillFileAsync(IEnumerable<Student> students)
        {
            StringBuilder line = new StringBuilder();

            foreach (var s in students)
            {
                line.Append(s.Account.FirstName + ';');
                line.Append(s.Account.LastName + ';');
                line.Append(s.Account.Email + ';' + '\n');
            }

            byte[] byteLine = line.ToString().ConvertLineToArray();

            await _memoryStream.WriteAsync(byteLine);
        }

        public override string GetFileName()
        {
            return $"{_fileGroupName}.csv";
        }
    }
}

