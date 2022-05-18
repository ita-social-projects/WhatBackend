using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class StudentGroupsHtmlFileExport : FileExport<IEnumerable<Student>>
    {
        #region private
        private readonly string _fileGroupName;
        private const int FirstNameColumnNumber = 0;
        private const int LastNameColumnNumber = 1;
        private const int EmailColumnNumber = 2;
        #endregion

        public StudentGroupsHtmlFileExport(string fileGroupName)
        {
            _fileGroupName = fileGroupName;
        }

        public override async Task FillFileAsync(IEnumerable<Student> students)
        {
            string[] headers = new string[] { "FirstName", "Lastname", "Email" };

            string[][] rows = new string[students.Count()][];

            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new string[headers.Length];
                rows[i][FirstNameColumnNumber] = students.ElementAt(i).Account.FirstName;
                rows[i][LastNameColumnNumber] = students.ElementAt(i).Account.LastName;
                rows[i][EmailColumnNumber] = students.ElementAt(i).Account.Email;
            }

            StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);

            StringBuilder html = HtmlGenerator.GenerateHtml(_fileGroupName, table);

            byte[] byteLine = html.ToString().ConvertLineToArray();

            await _memoryStream.WriteAsync(byteLine);
        }

        public override string GetFileName()
        {
            return $"{_fileGroupName} | {DateTime.Now.ToString("dd-MM-yyyy")}.html";
        }
    }
}
