#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Dashboard;
using System.Threading.Tasks;

#endregion

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public class ClassbookHtmlFileExport : FileExport<StudentsClassbookResultDto>
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
                IEnumerable<IGrouping<long?, StudentVisitDto>> dateData =
                    data.StudentsPresences.GroupBy(x => x.LessonId);

                string[] headers = new string[dateData.Count() + 1];
                headers[0] = "Students:";

                for (int i = 0; i + 1 < headers.Length; i++)
                {
                    headers[i + 1] =
                        ((DateTime) data.StudentsPresences.First(x => x.LessonId == dateData.ElementAt(i).Key)
                            .LessonDate).ToString("dd.MM.yyyy");
                }

                IOrderedEnumerable<string> students =
                    data.StudentsPresences.Select(x => x.Student).Distinct().OrderByDescending(s => s);

                string[][] rows = new string[students.Count()][];

                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = new string[headers.Length];
                    rows[i][0] = students.ElementAt(i);

                    for (int j = 0; j + 1 < headers.Length; j++)
                    {
                        IEnumerable<StudentVisitDto> visits = data.StudentsPresences
                            .Where(x => x.Student == rows[i][0] && x.LessonDate?
                                .ToString("dd.MM.yyyy") == headers[j + 1]);

                        bool? studentVisit = visits.Any() ? visits.First().Presence : null;

                        rows[i][j + 1] = studentVisit == null ? " " :
                            studentVisit == true ? $"{HtmlConstants.CheckMark}" : " ";
                    }
                }

                StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);

                StringBuilder html = HtmlGenerator.GenerateHtml(GetFileHeader(data), table);

                byte[] byteLine = html.ToString().ConvertLineToArray();

                _memoryStream.Write(byteLine);
            }
        }

        private void FillMarks(StudentsClassbookResultDto data)
        {
            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                IEnumerable<IGrouping<long?, StudentMarkDto>> dateData = data.StudentsMarks.GroupBy(x => x.LessonId);

                //Need 2 extra slots for columns "students" and "average mark" (besides lesson dates)
                string[] headers = new string[dateData.Count() + 2];
                headers[0] = "Students:";
                headers[dateData.Count() + 1] = "Avg:";

                for (int i = 0; i + 1 < headers.Length - 1; i++)
                {
                    headers[i + 1] =
                        ((DateTime) data.StudentsMarks.First(x => x.LessonId == dateData.ElementAt(i).Key).LessonDate)
                        .ToString("dd.MM.yyyy");
                }

                IOrderedEnumerable<string> students =
                    data.StudentsMarks.Select(x => x.Student).Distinct().OrderByDescending(s => s);

                string[][] rows = new string[students.Count()][];

                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = new string[headers.Length];
                    rows[i][0] = students.ElementAt(i);

                    var marks = new List<int>();

                    for (int j = 0; j + 1 < headers.Length - 1; j++)
                    {
                        IEnumerable<StudentMarkDto> studentMarks = data.StudentsMarks
                            .Where(x => x.Student == rows[i][0] && x.LessonDate?
                                .ToString("dd.MM.yyyy") == headers[j + 1]);

                        sbyte? studentMark = studentMarks.Any() ? studentMarks.First().StudentMark : null;

                        if (studentMark != null && studentMark != 0)
                        {
                            marks.Add((int) studentMark);
                        }

                        rows[i][j + 1] = studentMark == null ? " " : studentMark == 0 ? "-" : $"{studentMark}";
                    }

                    rows[i][headers.Length - 1] = string.Format("{0:0.00}", marks.Any() ? marks.Average() : 0f);
                }

                StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);

                StringBuilder html = HtmlGenerator.GenerateHtml(GetFileHeader(data), table);

                byte[] byteLine = html.ToString().ConvertLineToArray();

                _memoryStream.Write(byteLine);
            }
        }

        private string GetFileHeader(StudentsClassbookResultDto data)
        {
            string group = string.Empty;
            string startDate = string.Empty;
            string finishDate = string.Empty;

            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                group = data.StudentsPresences.First().StudentGroup;
                startDate = data.StudentsPresences.First().LessonDate?.ToString("dd.MM.yyyy");
                finishDate = data.StudentsPresences.Last().LessonDate?.ToString("dd.MM.yyyy");
            }
            else if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                group = data.StudentsMarks.First().StudentGroup;
                startDate = data.StudentsMarks.First().LessonDate?.ToString("dd.MM.yyyy");
                finishDate = data.StudentsMarks.Last().LessonDate?.ToString("dd.MM.yyyy");
            }

            return $"Group: {group} {startDate}{HtmlConstants.NonBreakingSpace}{finishDate}";
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