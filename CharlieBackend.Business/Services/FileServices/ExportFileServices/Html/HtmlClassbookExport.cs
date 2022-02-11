using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    class HtmlClassbookExport : HtmlFileExport<StudentsClassbookResultDto>
    {
        private void FillFile(StudentsClassbookResultDto data)
        {
            if (data == null)
            {
                return;
            }

            FillPresences(data);

            FillMarks(data);
        }

        private void FillPresences(StudentsClassbookResultDto data)
        {
            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                var dateData = data.StudentsPresences.GroupBy(x => x.LessonId);

                string[] headers = new string[dateData.Count() + 1];
                headers[0] = "Students:";

                for (int i = 0; i + 1 < headers.Length; i++)
                {
                    headers[i + 1] = ((DateTime)data.StudentsPresences.First(x => x.LessonId == dateData.ElementAt(i).Key).LessonDate).ToString("dd.MM.yyyy");
                }

                var students = data.StudentsPresences.Select(x => x.Student).Distinct().OrderByDescending(s => s);

                string[][] rows = new string[students.Count()][];

                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = new string[headers.Length];
                    rows[i][0] = students.ElementAt(i);

                    for (int j = 0; j + 1 < headers.Length; j++)
                    {
                        var visits = data.StudentsPresences
                            .Where(x => x.Student == rows[i][0] && x.LessonDate?
                            .ToString("dd.MM.yyyy") == headers[j + 1]);

                        var studentVisit = visits.Any() ? visits.First().Presence : null;

                        rows[i][j + 1] = studentVisit == null ? " " : studentVisit == true ? $"&#10003;" : " ";
                    }
                }

                StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);

                StringBuilder html = HtmlGenerator.GenerateHtml(GetFileHeader(data), table);

                byte[] byteLine = ConvertLineToArray(html.ToString());

                _memoryStream.Write(byteLine);
            }
        }

        private void FillMarks(StudentsClassbookResultDto data)
        {
            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                var dateData = data.StudentsMarks.GroupBy(x => x.LessonId);

                //Need 2 extra slots for columns "students" and "average mark" (besides lesson dates)
                string[] headers = new string[dateData.Count() + 2];
                headers[0] = "Students:";
                headers[dateData.Count() + 1] = "Avg:";

                for (int i = 0; i + 1 < headers.Length - 1; i++)
                {
                    headers[i + 1] = ((DateTime)data.StudentsMarks.First(x => x.LessonId == dateData.ElementAt(i).Key).LessonDate).ToString("dd.MM.yyyy");
                }

                var students = data.StudentsMarks.Select(x => x.Student).Distinct().OrderByDescending(s => s);

                string[][] rows = new string[students.Count()][];

                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = new string[headers.Length];
                    rows[i][0] = students.ElementAt(i);

                    List<int> marks = new List<int>();

                    for (int j = 0; j + 1 < headers.Length - 1; j++)
                    {
                        var studentMarks = data.StudentsMarks
                            .Where(x => x.Student == rows[i][0] && x.LessonDate?
                            .ToString("dd.MM.yyyy") == headers[j + 1]);

                        var studentMark = studentMarks.Any() ? studentMarks.First().StudentMark : null;

                        if (studentMark != null && studentMark != 0)
                        {
                            marks.Add((int)studentMark);
                        }

                        rows[i][j + 1] = studentMark == null ? " " : studentMark == 0 ? "-" : $"{studentMark}";
                    }

                    rows[i][headers.Length - 1] = string.Format("{0:0.00}", marks.Any() ? marks.Average() : 0f);
                }

                StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);

                StringBuilder html = HtmlGenerator.GenerateHtml(GetFileHeader(data), table);

                byte[] byteLine = ConvertLineToArray(html.ToString());

                _memoryStream.Write(byteLine);
            }
        }

        private string GetFileHeader(StudentsClassbookResultDto data)
        {
            string group = String.Empty;
            string startDate = String.Empty;
            string finishDate = String.Empty;

            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                group = data.StudentsPresences.First().StudentGroup;
                startDate = data.StudentsPresences.First().LessonDate?.ToString("dd.MM.yyyy");
                finishDate = data.StudentsPresences.Last().LessonDate?.ToString("dd.MM.yyyy");
            }
            else if(data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                group = data.StudentsMarks.First().StudentGroup;
                startDate = data.StudentsMarks.First().LessonDate?.ToString("dd.MM.yyyy");
                finishDate = data.StudentsMarks.Last().LessonDate?.ToString("dd.MM.yyyy");
            }

            return $"Group: {group} {startDate}{HtmlGenerator.NonBreakingSpace}{finishDate}";
        }

        public override string GetFileName()
        {
            return "Classbook_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html";
        }

        public override async Task FillFileAsync(StudentsClassbookResultDto data)
        {
            await Task.Run(()=>FillFile(data));
        }
    }
}
