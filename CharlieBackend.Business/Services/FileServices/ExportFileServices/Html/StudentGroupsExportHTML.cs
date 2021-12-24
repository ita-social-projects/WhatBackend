using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class StudentGroupsExportHTML : BaseFileExportHTML
    {
        private string _fileGroupName;

        public StudentGroupsExportHTML(string fileGroupName)
        {
            _fileGroupName = fileGroupName;
        }

        public Task FillFile(IEnumerable<Student> students)
        {
            StringBuilder tableHead = new StringBuilder("<tr>");
            tableHead.Append(HtmlGenerator.HeadTh("#"));
            tableHead.Append(HtmlGenerator.HeadTh("FirstName"));
            tableHead.Append(HtmlGenerator.HeadTh("LastName"));
            tableHead.Append(HtmlGenerator.HeadTh("Email"));
            tableHead.Append("</tr>");

            StringBuilder tableBody = new StringBuilder();
            int counter = 1;
            foreach (var student in students)
            {
                tableBody.Append("<tr>");
                tableBody.Append(HtmlGenerator.BodyTh($"{counter}"));
                tableBody.Append($"<td>{student.Account.FirstName}</td>");
                tableBody.Append($"<td>{student.Account.LastName}</td>");
                tableBody.Append($"<td>{student.Account.Email}</td>");
                tableBody.Append("</tr>");
                counter++;
            }

            StringBuilder html = HtmlGenerator.GenerateHtml(_fileGroupName, tableHead.ToString(), tableBody.ToString());

            byte[] byteLine = ConvertLineToArray(html.ToString());

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
            return $"{_fileGroupName} | {DateTime.Now.ToString("dd-MM-yyyy")}.html";
        }
    }
}
