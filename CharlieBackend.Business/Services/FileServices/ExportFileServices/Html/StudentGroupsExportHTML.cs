﻿using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class StudentGroupsExportHTML : BaseFileExportHTML
    {
        private string _fileGroupName;
        private const int _FirstNameColumnNumber = 0;
        private const int _LastNameColumnNumber = 1;
        private const int _EmailColumnNumber = 2;

        public StudentGroupsExportHTML(string fileGroupName)
        {
            _fileGroupName = fileGroupName;
        }

        public void FillFile(IEnumerable<Student> students)
        {
            string[] headers = new string[] { "FirstName", "Lastname", "Email" };

            string[][] rows = new string[students.Count()][];

            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new string[headers.Length];
                rows[i][_FirstNameColumnNumber] = students.ElementAt(i).Account.FirstName;
                rows[i][_LastNameColumnNumber] = students.ElementAt(i).Account.LastName;
                rows[i][_EmailColumnNumber] = students.ElementAt(i).Account.Email;
            }

            StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);

            StringBuilder html = HtmlGenerator.GenerateHtml(_fileGroupName, table);

            byte[] byteLine = ConvertLineToArray(html.ToString());

            _memoryStream.Write(byteLine);
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
