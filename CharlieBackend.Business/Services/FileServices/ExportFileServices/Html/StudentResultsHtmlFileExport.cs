using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    class StudentResultsHtmlFileExport : FileExport<StudentsResultsDto>
    {
        #region private constants

        private const int CourseColumnNumber = 0;
        private const int StudentGroupColumnNumber = 1;
        private const int ThirdColumnNumber = 2;
        private const int FourthColumn = 3;

        #endregion

        private void FillFile(StudentsResultsDto data)
        {
            if (data == null)
            {
                return;
            }

            var table = new StringBuilder();

            bool averageVisitsExistAndValid = data.AverageStudentVisits != null && data.AverageStudentVisits.Any();

            bool averageMarksExistAndValid = data.AverageStudentsMarks != null && data.AverageStudentsMarks.Any();

            if (averageMarksExistAndValid && averageVisitsExistAndValid)
            {
                IEnumerable<AverageStudentMarkDto> studentMarks = data.AverageStudentsMarks;

                IEnumerable<AverageStudentVisitsDto> studentVisits = data.AverageStudentVisits;

                string[] headers = { "Course", "Student Group", "Average Mark", "Average visits percentage" };

                string[][] rows = new string[studentMarks.Count()][];

                for (int i = 0; i < studentMarks.Count(); i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][CourseColumnNumber] = studentMarks.ElementAt(i).Course;
                    rows[i][StudentGroupColumnNumber] = studentMarks.ElementAt(i).StudentGroup;
                    rows[i][ThirdColumnNumber] = string.Format("{0:0.0}", studentMarks.ElementAt(i).StudentAverageMark);
                    rows[i][FourthColumn] = $"{studentVisits.ElementAt(i).StudentAverageVisitsPercentage}";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (averageVisitsExistAndValid)
            {
                IEnumerable<AverageStudentVisitsDto> studentVisits = data.AverageStudentVisits;

                string[] headers = { "Course", "Student Group", "Average visits percentage" };

                string[][] rows = new string[studentVisits.Count()][];

                for (int i = 0; i < studentVisits.Count(); i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][CourseColumnNumber] = studentVisits.ElementAt(i).Course;
                    rows[i][StudentGroupColumnNumber] = studentVisits.ElementAt(i).StudentGroup;
                    rows[i][ThirdColumnNumber] = $"{studentVisits.ElementAt(i).StudentAverageVisitsPercentage}";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (averageMarksExistAndValid)
            {
                IEnumerable<AverageStudentMarkDto> studentMarks = data.AverageStudentsMarks;

                string[] headers = { "Course", "Student Group", "Average Mark" };

                string[][] rows = new string[studentMarks.Count()][];

                for (int i = 0; i < studentMarks.Count(); i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][CourseColumnNumber] = studentMarks.ElementAt(i).Course;
                    rows[i][StudentGroupColumnNumber] = studentMarks.ElementAt(i).StudentGroup;
                    rows[i][ThirdColumnNumber] = string.Format("{0:0.0}", studentMarks.ElementAt(i).StudentAverageMark);
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }

            StringBuilder html = HtmlGenerator.GenerateHtml(GetFileHeader(data), table);

            byte[] byteLine = html.ToString().ConvertLineToArray();

            _memoryStream.Write(byteLine);
        }

        private string GetFileHeader(StudentsResultsDto data)
        {
            string student = string.Empty;
            string startDate = string.Empty;
            string finishDate = string.Empty;

            if (data.AverageStudentVisits != null && data.AverageStudentVisits.Any())
            {
                student = data.AverageStudentVisits.First().Student;
            }
            else if (data.AverageStudentsMarks != null && data.AverageStudentsMarks.Any())
            {
                student = data.AverageStudentsMarks.First().Student;
            }

            return $"Student: {student} average results";
        }

        public override async Task FillFileAsync(StudentsResultsDto data)
        {
            await Task.Run(() => FillFile(data));
        }

        public override string GetFileName()
        {
            return "StudentResults_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html";
        }
    }
}
