using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.Constants;
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

        public override string GetFileName()
        {
            return HtmlFileExportConstants.StudentsResultsFileNameConstant(DateTime.Now.ToString("yyyy-MM-dd"));
        }

        public override async Task FillFileAsync(StudentsResultsDto data)
        {
            if (data == null)
            {
                return;
            }

            var table = new StringBuilder();

            IList<AverageStudentMarkDto> studentMarks = data.AverageStudentsMarks.ToList();

            IList<AverageStudentVisitsDto> studentVisits = data.AverageStudentVisits.ToList();

            if (data.AverageStudentsMarks.Any() && data.AverageStudentVisits.Any())
            {
                string[] headers = { "Course", "Student Group", "Average Mark", "Average visits" };

                string[][] rows = new string[studentMarks.Count][];

                for (int i = 0; i < studentMarks.Count; i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][CourseColumnNumber] = studentMarks[i].Course;
                    rows[i][StudentGroupColumnNumber] = studentMarks[i].StudentGroup;
                    rows[i][ThirdColumnNumber] = string.Format("{0:0.0}", studentMarks[i].StudentAverageMark);
                    rows[i][FourthColumn] = $"{studentVisits[i].StudentAverageVisitsPercentage}%";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (data.AverageStudentVisits.Any())
            {
                string[] headers = { "Course", "Student Group", "Average visits" };

                int studentVisitsCount = studentVisits.Count();

                string[][] rows = new string[studentVisitsCount][];

                for (int i = 0; i < studentVisitsCount; i++)
                {
                    rows[i] = new string[headers.Length];

                    AverageStudentVisitsDto studentVisit = studentVisits.ElementAt(i);

                    rows[i][CourseColumnNumber] = studentVisit.Course;
                    rows[i][StudentGroupColumnNumber] = studentVisit.StudentGroup;
                    rows[i][ThirdColumnNumber] = $"{studentVisit.StudentAverageVisitsPercentage}%";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (data.AverageStudentsMarks.Any())
            {
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

            await _memoryStream.WriteAsync(byteLine);
        }

        private string GetFileHeader(StudentsResultsDto data)
        {
            string studentName = data.AverageStudentVisits.Any()
                    ? data.AverageStudentVisits.First().Student
                    : data.AverageStudentsMarks.First().Student;

            return HtmlFileExportConstants.StudentsResultsFileHeaderConstant(studentName);
        }
    }
}
