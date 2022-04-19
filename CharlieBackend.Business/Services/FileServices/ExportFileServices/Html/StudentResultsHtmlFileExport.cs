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

                int studentMarksCount = studentMarks.Count();

                string[][] rows = new string[studentMarksCount][];

                for (int i = 0; i < studentMarksCount; i++)
                {
                    rows[i] = new string[headers.Length];

                    AverageStudentMarkDto studentMark = studentMarks.ElementAt(i);

                    rows[i][CourseColumnNumber] = studentMark.Course;
                    rows[i][StudentGroupColumnNumber] = studentMark.StudentGroup;
                    rows[i][ThirdColumnNumber] = string.Format("{0:0.0}", studentMark.StudentAverageMark);
                    rows[i][FourthColumn] = $"{studentVisits.ElementAt(i).StudentAverageVisitsPercentage}";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (averageVisitsExistAndValid)
            {
                IEnumerable<AverageStudentVisitsDto> studentVisits = data.AverageStudentVisits;

                string[] headers = { "Course", "Student Group", "Average visits percentage" };

                int studentVisitsCount = studentVisits.Count();

                string[][] rows = new string[studentVisitsCount][];

                for (int i = 0; i < studentVisitsCount; i++)
                {
                    rows[i] = new string[headers.Length];

                    AverageStudentVisitsDto studentVisit = studentVisits.ElementAt(i);

                    rows[i][CourseColumnNumber] = studentVisit.Course;
                    rows[i][StudentGroupColumnNumber] = studentVisit.StudentGroup;
                    rows[i][ThirdColumnNumber] = $"{studentVisit.StudentAverageVisitsPercentage}";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (averageMarksExistAndValid)
            {
                IEnumerable<AverageStudentMarkDto> studentMarks = data.AverageStudentsMarks;

                string[] headers = { "Course", "Student Group", "Average Mark" };

                int studentMarksCount = studentMarks.Count();

                string[][] rows = new string[studentMarksCount][];

                for (int i = 0; i < studentMarksCount; i++)
                {
                    rows[i] = new string[headers.Length];

                    AverageStudentMarkDto studentMark = studentMarks.ElementAt(i);

                    rows[i][CourseColumnNumber] = studentMark.Course;
                    rows[i][StudentGroupColumnNumber] = studentMark.StudentGroup;
                    rows[i][ThirdColumnNumber] = string.Format("{0:0.0}", studentMark.StudentAverageMark);
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
