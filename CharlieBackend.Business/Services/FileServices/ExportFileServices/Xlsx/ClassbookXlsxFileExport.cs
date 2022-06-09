using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ClassbookXlsxFileExport : XlsxFileExport<StudentsClassbookResultDto>
    {
        public override string GetFileName()
        {
            return "Classbook_" + DateTime.Now.ToString("yyyy-MM-dd.HH:mm") + ".xlsx";
        }

        private async Task TryToFillPresencesAsync(StudentsClassbookResultDto data)
        {
            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                var firstStudentPresence = data.StudentsPresences.First();
                string workhseetName = "Presence (" + (_xLWorkbook.Worksheets.Count + 1) + ")";
                _xLWorkbook.AddWorksheet(workhseetName);
                var worksheet = _xLWorkbook.Worksheet(workhseetName);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Student:");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   firstStudentPresence.Course,
                   firstStudentPresence.StudentGroup);

                FillRow(worksheet, 4, 1, "❌ - student is present");

                var dateData = data.StudentsPresences.GroupBy(x => x.LessonId);

                var studentList = data.StudentsPresences.Select(x => x.Student).Distinct().OrderByDescending(s => s).ToList();

                for (int studentIndex = 0; studentIndex < studentList.Count(); studentIndex++)
                {
                    worksheet.Cell(_STUDENT_STARTING_ROW + studentIndex, _STUDENT_STARTING_COLUMN)
                        .Value = studentList.ElementAt(studentIndex);
                }

                var dateColumn = _DEFAULT_STARTING_COLUMN;

                foreach (var date in dateData)
                {
                    date.OrderByDescending(s => s.Student);

                    worksheet.Row(1)
                       .Cell(dateColumn)
                       .Value = ((DateTime)data.StudentsPresences.First(x => x.LessonId == dateData.ElementAt(dateColumn - _DEFAULT_STARTING_COLUMN).Key).LessonDate).ToString("dd-MM-yyyy");

                    foreach (var visit in date)
                    {
                        FillRow(worksheet,
                            _STUDENT_STARTING_ROW + studentList.IndexOf(visit.Student),
                            dateColumn,
                            visit.Presence == null ? " " : visit.Presence == true ? "❌" : " ");
                    }

                    dateColumn++;
                }

                DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN),
                        worksheet.Row(studentList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN + dateData.Count())));

                DrawBorders(worksheet.Range("A1:B2"));
                DrawBorders(worksheet.Range("A4:A4"));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }

        private async Task TryToFillMarksAsync(StudentsClassbookResultDto data)
        {
            var firstStudentMark = data.StudentsMarks.First();
            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                string workhseetName = "Marks (" + (_xLWorkbook.Worksheets.Count + 1) + ")";
                _xLWorkbook.AddWorksheet(workhseetName);
                var worksheet = _xLWorkbook.Worksheet(workhseetName);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Student:");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   firstStudentMark.Course,
                   firstStudentMark.StudentGroup);

                var dateData = data.StudentsMarks.GroupBy(x => x.LessonId);

                var studentList = data.StudentsMarks.Select(x => x.Student).Distinct().OrderByDescending(s => s).ToList();

                for (int studentIndex = 0; studentIndex < studentList.Count(); studentIndex++)
                {
                    worksheet.Cell(_STUDENT_STARTING_ROW + studentIndex, _STUDENT_STARTING_COLUMN)
                        .Value = studentList.ElementAt(studentIndex);
                }

                var dateColumn = _DEFAULT_STARTING_COLUMN;

                foreach (var date in dateData)
                {
                    date.OrderByDescending(s => s.Student);

                    worksheet.Row(1)
                       .Cell(dateColumn)
                       .Value = ((DateTime)data.StudentsMarks.First(x => x.LessonId == dateData.ElementAt(dateColumn - _DEFAULT_STARTING_COLUMN).Key).LessonDate).ToString("dd-MM-yyyy");

                    foreach (var mark in date)
                    {
                        FillRow(worksheet,
                            _STUDENT_STARTING_ROW + studentList.IndexOf(mark.Student),
                            dateColumn,
                            mark.StudentMark == null ? " " : mark.StudentMark.ToString());
                    }

                    dateColumn++;
                }

                DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN),
                        worksheet.Row(studentList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN + dateData.Count())));

                DrawBorders(worksheet.Range("A1:B2"));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }

        public async override ValueTask FillFileAsync(StudentsClassbookResultDto data)
        {
            if (data == null)
            {
                return;
            }

            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                var StudentsMarks = data.StudentsMarks.GroupBy(x => x.StudentGroup);
                var listTasks = new List<Task>();
                foreach (var item in StudentsMarks)
                {
                    listTasks.Add(
                    TryToFillMarksAsync(new StudentsClassbookResultDto
                    {
                        StudentsMarks = item
                             .Select(x => new StudentMarkDto
                             {
                                 LessonDate = x.LessonDate,
                                 LessonId = x.LessonId,
                                 Course = x.Course,
                                 StudentMark = x.StudentMark,
                                 Student = x.Student,
                                 StudentGroup = x.StudentGroup,
                                 StudentId = x.StudentId,
                                 Comment = x.Comment
                             })
                            .ToList(),
                        StudentsPresences = null
                    })
                    );
                }

                await Task.WhenAll(listTasks);
            }

            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                var StudentsPresences = data.StudentsPresences.GroupBy(x => x.StudentGroup);
                var listTasks = new List<Task>();
                foreach (var item in StudentsPresences)
                {
                    listTasks.Add(
                    TryToFillPresencesAsync(new StudentsClassbookResultDto
                    {
                        StudentsMarks = null,
                        StudentsPresences = item
                            .Select(x => new StudentVisitDto
                            {
                                LessonDate = x.LessonDate,
                                LessonId = x.LessonId,
                                Course = x.Course,
                                Presence = x.Presence,
                                Student = x.Student,
                                StudentGroup = x.StudentGroup,
                                StudentId = x.StudentId
                            })
                            .ToList()
                    })
                    );
                }

                await Task.WhenAll(listTasks);
            }
        }
    }
}
