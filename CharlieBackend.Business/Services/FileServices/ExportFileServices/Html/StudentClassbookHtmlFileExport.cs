using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Dashboard;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class StudentClassbookHtmlFileExport : FileExport<StudentsClassbookResultDto>
    {
        #region private constants

        private const int CourseColumnNumber = 0;
        private const int StudentGroupColumnNumber = 1;
        private const int DateColumnNumber = 2;
        private const int FourthColumn = 3;
        private const int FifthColumn = 4;

        #endregion

        public override async Task FillFileAsync(StudentsClassbookResultDto data)
        {
            if (data == null)
            {
                return;
            }

            var table = new StringBuilder();

            bool visitsExistAndValid = data.StudentsPresences != null && data.StudentsPresences.Any();

            bool marksExistAndValid = data.StudentsMarks != null && data.StudentsMarks.Any();

            if (marksExistAndValid && visitsExistAndValid)
            {
                IEnumerable<StudentMarkDto> studentMarks = data.StudentsMarks;

                IEnumerable<StudentVisitDto> studentVisits = data.StudentsPresences;

                string[] headers = {"Course", "Student Group", "Lesson date", "Presence", "Marks"};

                string[][] rows = new string[data.StudentsMarks.Count()][];

                for (int i = 0; i < studentMarks.Count(); i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][CourseColumnNumber] = studentMarks.ElementAt(i).Course;
                    rows[i][StudentGroupColumnNumber] = studentMarks.ElementAt(i).StudentGroup;
                    rows[i][DateColumnNumber] = studentMarks.ElementAt(i).LessonDate.ToString();
                    rows[i][FourthColumn] = studentVisits.ElementAt(i).Presence == null ? " " :
                        studentVisits.ElementAt(i).Presence == true ? $"{HtmlConstants.CheckMark}" : " ";
                    rows[i][FifthColumn] = studentMarks.ElementAt(i).StudentMark == null ? " " :
                        studentMarks.ElementAt(i).StudentMark == 0 ? "-" : $"{studentMarks.ElementAt(i).StudentMark}";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (visitsExistAndValid)
            {
                IEnumerable<StudentVisitDto> studentVisits = data.StudentsPresences;

                string[] headers = {"Course", "Student Group", "Lesson date", "Presence"};

                string[][] rows = new string[data.StudentsPresences.Count()][];

                for (int i = 0; i < studentVisits.Count(); i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][CourseColumnNumber] = studentVisits.ElementAt(i).Course;
                    rows[i][StudentGroupColumnNumber] = studentVisits.ElementAt(i).StudentGroup;
                    rows[i][DateColumnNumber] = studentVisits.ElementAt(i).LessonDate.ToString();
                    rows[i][FourthColumn] = studentVisits.ElementAt(i).Presence == null ? " " :
                        studentVisits.ElementAt(i).Presence == true ? $"{HtmlConstants.CheckMark}" : " ";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }
            else if (marksExistAndValid)
            {
                IEnumerable<StudentMarkDto> studentMarks = data.StudentsMarks;

                string[] headers = {"Course", "Student Group", "Lesson date", "Marks"};

                string[][] rows = new string[data.StudentsMarks.Count()][];

                for (int i = 0; i < studentMarks.Count(); i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][CourseColumnNumber] = studentMarks.ElementAt(i).Course;
                    rows[i][StudentGroupColumnNumber] = studentMarks.ElementAt(i).StudentGroup;
                    rows[i][DateColumnNumber] = studentMarks.ElementAt(i).LessonDate.ToString();
                    rows[i][FourthColumn] = studentMarks.ElementAt(i).StudentMark == null ? " " :
                        studentMarks.ElementAt(i).StudentMark == 0 ? "-" : $"{studentMarks.ElementAt(i).StudentMark}";
                }

                table = HtmlGenerator.GenerateTable(headers, rows);
            }

            StringBuilder html = HtmlGenerator.GenerateHtml(GetFileHeader(data), table);

            byte[] byteLine = html.ToString().ConvertLineToArray();

            await _memoryStream.WriteAsync(byteLine);
        }

        private string GetFileHeader(StudentsClassbookResultDto data)
        {
            string student = string.Empty;
            string startDate = string.Empty;
            string finishDate = string.Empty;

            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                student = data.StudentsPresences.First().Student;
                startDate = data.StudentsPresences.First().LessonDate?.ToString("dd.MM.yyyy");
                finishDate = data.StudentsPresences.Last().LessonDate?.ToString("dd.MM.yyyy");
            }
            else if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                student = data.StudentsMarks.First().Student;
                startDate = data.StudentsMarks.First().LessonDate?.ToString("dd.MM.yyyy");
                finishDate = data.StudentsMarks.Last().LessonDate?.ToString("dd.MM.yyyy");
            }

            return $"Student {student} results {startDate}{HtmlConstants.NonBreakingSpace}{finishDate}";
        }

        public override string GetFileName()
        {
            return "StudentClassbookResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html";
        }
    }
}