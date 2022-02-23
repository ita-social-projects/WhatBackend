using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    class StudentClassbookExportHTML : BaseFileExportHTML
    {
        #region private constants
        private const int _CourseColumnNumber = 0;
        private const int _StudentGroupColumnNumber = 1;
        private const int _DateColumnNumber = 2;
        private const int _PressenceColumnNumber = 3;
        #endregion

        public void FillFile(StudentsClassbookResultDto data)
        {
            if (data == null)
            {
                return;
            }

            FillPresences(data);
        }

        private void FillPresences(StudentsClassbookResultDto data)
        {
            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                var studentVisits = data.StudentsPresences;

                string[] headers = new string[] { "Course", "Student Group", "Lesson date", "Presence" };

                string[][] rows = new string[data.StudentsPresences.Count()][];

                for (int i = 0; i < studentVisits.Count(); i++)
                {
                    rows[i] = new string[headers.Length];

                    rows[i][_CourseColumnNumber] = studentVisits.ElementAt(i).Course;
                    rows[i][_StudentGroupColumnNumber] = studentVisits.ElementAt(i).StudentGroup;
                    rows[i][_DateColumnNumber] = studentVisits.ElementAt(i).LessonDate.ToString();
                    rows[i][_PressenceColumnNumber] = studentVisits.ElementAt(i).Presence == null ? " " : studentVisits.ElementAt(i).Presence == true ? $"&#10003;" : " ";
                }

                StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);

                StringBuilder html = HtmlGenerator.GenerateHtml(GetFileHeader(data), table);

                byte[] byteLine = ConvertLineToArray(html.ToString());

                _memoryStream.Write(byteLine);
            }
        }

        private string GetFileHeader(StudentsClassbookResultDto data)
        {
            string student = String.Empty;
            string startDate = String.Empty;
            string finishDate = String.Empty;

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

            return $"Student {student} results {startDate}{HtmlGenerator.NonBreakingSpace}{finishDate}";
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
            return "StudentClassbookResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html";
        }
    }
}
