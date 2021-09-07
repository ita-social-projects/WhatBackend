using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ClassbookExportXlsx : BaseFileExportXlsx
    {
        public ClassbookExportXlsx()
        {
            xLWorkbook = new XLWorkbook();
        }

        public override string GetFileName()
        {
            return "Classbook_" + DateTime.Now.ToString("yyyy-MM-dd.HH:mm") + ".xlsx";
        }

        private async Task TryToFillPresences(StudentsClassbookResultDto data)
        {
            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                var firstStudentPresence = data.StudentsPresences.First();
                string workhseetName = "Presence (" + (xLWorkbook.Worksheets.Count + 1) + ")";
                xLWorkbook.AddWorksheet(workhseetName);
                var worksheet = xLWorkbook.Worksheet(workhseetName);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Student:");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   firstStudentPresence.Course,
                   firstStudentPresence.StudentGroup);

                FillRow(worksheet, 4, 1, "❌ - student is present");

                var dateData = data.StudentsPresences.GroupBy(x => x.LessonDate);

                var studentList = dateData.First().OrderBy(x => x.Student);
                for (int studentIndex = 0; studentIndex < studentList.Count(); studentIndex++)
                {
                    worksheet.Cell(_STUDENT_STARTING_ROW + studentIndex, _STUDENT_STARTING_COLUMN)
                        .Value = studentList.ElementAt(studentIndex).Student;
                }

                int presencesGroupsCount = dateData.Count();

                for (int groupN = 0; groupN < presencesGroupsCount; groupN++)
                {
                    int actualIndex = groupN + _DEFAULT_STARTING_COLUMN;
                    var actualGroup = dateData.ElementAt(groupN).OrderBy(x => x.Student);

                    worksheet.Row(1)
                        .Cell(actualIndex)
                        .Value = ((DateTime)dateData.ElementAt(groupN).Key).ToString("dd-MM-yyyy");

                    var visitCount = actualGroup.Count();

                    for (int visitN = 0; visitN < visitCount; visitN++)
                    {
                        var group = actualGroup;
                        FillRowTextAlignCenter(worksheet, visitN + _STUDENT_STARTING_ROW, actualIndex,
                        group.ElementAt(visitN).Presence == true ? "❌" : " ");
                    }
                    
                    
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

        private async Task TryToFillMarks(StudentsClassbookResultDto data)
        {
            var firstStudentMark = data.StudentsMarks.First();
            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                string workhseetName = "Marks (" + (xLWorkbook.Worksheets.Count + 1) + ")";
                xLWorkbook.AddWorksheet(workhseetName);
                var worksheet = xLWorkbook.Worksheet(workhseetName);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Student:");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   firstStudentMark.Course,
                   firstStudentMark.StudentGroup);

                var dateData = data.StudentsMarks.GroupBy(x => x.LessonDate);

                var studentList = dateData.First().OrderBy(x => x.Student);
                for (int studentIndex = 0; studentIndex < studentList.Count(); studentIndex++)
                {
                    worksheet.Cell(_STUDENT_STARTING_ROW + studentIndex, _STUDENT_STARTING_COLUMN)
                        .Value = studentList.ElementAt(studentIndex).Student;
                }

                int markGroupsCount = dateData.Count();

                for (int groupN = 0; groupN < markGroupsCount; groupN++)
                {
                    int actualIndex = groupN + _DEFAULT_STARTING_COLUMN;
                    var actualGroup = dateData.ElementAt(groupN).OrderBy(x => x.Student);

                    worksheet.Row(1)
                        .Cell(actualIndex)
                        .Value = ((DateTime)dateData.ElementAt(groupN).Key).ToString("dd-MM-yyyy");

                    for (int item = 0; item < actualGroup.Count(); item++)
                    {
                        var group = actualGroup;

                        sbyte? tempMark = group.ElementAt(item).StudentMark != 0 ? group.ElementAt(item).StudentMark : null;

                        string tempComment = group.ElementAt(item).Comment == null || group.ElementAt(item).Comment == ""
                            ? null : group.ElementAt(item).Comment.ToString();

                        FillRow(worksheet,
                            item + _STUDENT_STARTING_ROW,
                            actualIndex,
                            tempMark.ToString());

                        FillRowWithComments(worksheet, 
                            item + _STUDENT_STARTING_ROW, 
                            actualIndex,
                            tempComment);
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
            if (data == null)
            {
                return;
            }

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
