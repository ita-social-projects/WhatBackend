using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ClassbookExport : BaseFileExport
    {
        public ClassbookExport()
        {
            xLWorkbook = new XLWorkbook();
        }

        public override string GetFileName()
        {
            return "Classbook_" + DateTime.Now.ToString("yyyy-MM-dd.HH:mm") + ".xlsx";
        }

        public async Task FillFileAlternate(StudentsClassbookResultDto data)
        {
            if (data.StudentsPresences != null)
            {
                var worksheet = xLWorkbook.Worksheet(1);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "",
                    "Student",
                    "Lesson date",
                    "Presence");

                FillRow(worksheet, 2, 1,
                    data.StudentsPresences.First().Course,
                    data.StudentsPresences.First().StudentGroup);

                for (int row = 0; row < data.StudentsPresences.Count(); row++)
                {
                    var presence = data.StudentsPresences.ElementAt(row);

                    FillRow(worksheet, row + _DEFAULT_STARTING_ROW, _DEFAULT_STARTING_COLUMN,
                        presence.Student,
                        presence.LessonDate != null ? ((DateTime)presence.LessonDate).ToString("yyyy-MM-dd") : "",
                        presence.Presence == true ? "Present" : "Absent");
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }

            if (data.StudentsMarks != null)
            {
                var worksheet = xLWorkbook.Worksheet(2);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "",
                    "Student",
                    "Lesson date",
                    "Student mark", 
                    "Comment");

                FillRow(worksheet, 2, 1,
                    data.StudentsMarks.First().Course,
                    data.StudentsMarks.First().StudentGroup);

                for (int row = 0; row < data.StudentsMarks.Count(); row++)
                {
                    var mark = data.StudentsMarks.ElementAt(row);

                    FillRow(worksheet, row + _DEFAULT_STARTING_ROW, _DEFAULT_STARTING_COLUMN,
                        mark.Student,
                        mark.LessonDate != null ? ((DateTime)mark.LessonDate).ToString("yyyy-MM-dd") : "",
                        mark.StudentMark.ToString(),
                        mark.Comment);
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }

        private async Task TryToFillPresences(StudentsClassbookResultDto data)
        {
            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                xLWorkbook.AddWorksheet("Presence of " + data.StudentsPresences.First().StudentGroup);
                var worksheet = xLWorkbook.Worksheet("Presence of " + data.StudentsPresences.First().StudentGroup);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Student:");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   data.StudentsPresences.First().Course,
                   data.StudentsPresences.First().StudentGroup);

                var dateData = data.StudentsPresences.GroupBy(x => x.LessonDate);

                var studentList = dateData.First().OrderBy(x => x.Student);
                for (int student = 0; student < studentList.Count(); student++)
                {
                    worksheet.Cell(_STUDENT_STARTING_ROW + student, _STUDENT_STARTING_COLUMN)
                        .Value = studentList.ElementAt(student).Student;
                }

                int presencesGroups = dateData.Count();

                for (int groupN = 0; groupN < presencesGroups; groupN++)
                {
                    int actualIndex = groupN + _DEFAULT_STARTING_COLUMN;
                    var actualGroup = dateData.ElementAt(groupN).OrderBy(x => x.Student);

                    worksheet.Row(1)
                        .Cell(actualIndex)
                        .Value = ((DateTime)dateData.ElementAt(groupN).Key).ToString("dd-MM-yyyy");

                    for (int item = 0; item < actualGroup.Count(); item++)
                    {
                        var group = actualGroup;
                        FillRow(worksheet, item + _STUDENT_STARTING_ROW, actualIndex,
                        group.ElementAt(item).Presence == true ? "+" : group.ElementAt(item).Presence == false ? "-" : " ");
                    }
                    
                    
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

        private async Task TryToFillMarks(StudentsClassbookResultDto data)
        {
            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                xLWorkbook.AddWorksheet("Marks of " + data.StudentsMarks.First().StudentGroup);
                var worksheet = xLWorkbook.Worksheet("Marks of " + data.StudentsMarks.First().StudentGroup);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Student:");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   data.StudentsMarks.First().Course,
                   data.StudentsMarks.First().StudentGroup);

                var dateData = data.StudentsMarks.GroupBy(x => x.LessonDate);

                var studentList = dateData.First().OrderBy(x => x.Student);
                for (int student = 0; student < studentList.Count(); student++)
                {
                    worksheet.Cell(_STUDENT_STARTING_ROW + student, _STUDENT_STARTING_COLUMN)
                        .Value = studentList.ElementAt(student).Student;
                }

                int presencesGroups = dateData.Count();

                for (int groupN = 0; groupN < presencesGroups; groupN++)
                {
                    int actualIndex = groupN + _DEFAULT_STARTING_COLUMN;
                    var actualGroup = dateData.ElementAt(groupN).OrderBy(x => x.Student);

                    worksheet.Row(1)
                        .Cell(actualIndex)
                        .Value = ((DateTime)dateData.ElementAt(groupN).Key).ToString("dd-MM-yyyy");

                    for (int item = 0; item < actualGroup.Count(); item++)
                    {
                        var group = actualGroup;
                        FillRow(worksheet, item + _STUDENT_STARTING_ROW, actualIndex,
                        group.ElementAt(item).StudentMark.ToString());
                        FillRowWithComments(worksheet, item + _STUDENT_STARTING_ROW, actualIndex,
                        group.ElementAt(item).Comment == null || group.ElementAt(item).Comment == ""
                        ? null : group.ElementAt(item).Comment.ToString());
                    }


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

        public async Task FillFile(StudentsClassbookResultDto data)
        {
            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                var StudentsMarks = data.StudentsMarks.GroupBy(x => x.StudentGroup);
                foreach (var item in StudentsMarks)
                {
                    await TryToFillMarks(new StudentsClassbookResultDto
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
                    });
                }
            }

            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                var StudentsPresences = data.StudentsPresences.GroupBy(x => x.StudentGroup);
                foreach (var item in StudentsPresences)
                {
                    await TryToFillPresences(new StudentsClassbookResultDto
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
                    });
                }
            }
        }
    }
}
