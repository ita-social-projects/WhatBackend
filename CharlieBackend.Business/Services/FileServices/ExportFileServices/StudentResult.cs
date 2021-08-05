using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class StudentResult : BaseFileExport
    {
        public StudentResult()
        {
            xLWorkbook = new XLWorkbook();
        }

        public override string GetFileName()
        {
            return "SingleStudentResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
        }

        public async Task FillFile(StudentsResultsDto data)
        {
            if (data.AverageStudentsMarks != null && data.AverageStudentsMarks.Any())
            {
                var StudentsAverageMarks = data.AverageStudentsMarks.GroupBy(x => x.Student);
                foreach (var item in StudentsAverageMarks)
                {
                    await FillAverageMarks(item
                        .Select(x => new AverageStudentMarkDto
                        {
                            StudentAverageMark = x.StudentAverageMark,
                            Course = x.Course,
                            Student = x.Student,
                            StudentGroup = x.StudentGroup
                        }
                        ));
                }
            }

            if (data.AverageStudentVisits != null && data.AverageStudentVisits.Any())
            {
                var StudentsPresences = data.AverageStudentVisits.GroupBy(x => x.Student);
                foreach (var item in StudentsPresences)
                {
                    await FillAverageVisits(item
                        .Select(x => new AverageStudentVisitsDto
                        {
                            StudentAverageVisitsPercentage = x.StudentAverageVisitsPercentage,
                            Course = x.Course,
                            Student = x.Student,
                            StudentGroup = x.StudentGroup
                        }
                        ));
                }
            }
        }

        private async Task FillAverageMarks(IEnumerable<AverageStudentMarkDto> AverageStudentsMarks)
        {
            if (AverageStudentsMarks != null && AverageStudentsMarks.Any())
            {
                xLWorkbook.AddWorksheet("Average marks of " + AverageStudentsMarks.First().Student);
                var worksheet = xLWorkbook.Worksheet("Average marks of " + AverageStudentsMarks.First().Student);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Student",
                    "Course",
                    "Student Group",
                    "Average mark");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   AverageStudentsMarks.First().Student);

                var orderedList = AverageStudentsMarks.OrderBy(x => x.Course).ThenBy(x => x.StudentGroup);
                for (int studentId = 0; studentId < orderedList.Count(); studentId++)
                {
                    var student = orderedList.ElementAt(studentId);

                    FillRow(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN - 1,
                        student.Course, student.StudentGroup, Math.Round(((decimal)student.StudentAverageMark), 2).ToString());
                }

                DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN - 1),
                        worksheet.Row(orderedList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN + 1)));

                DrawBorders(worksheet.Range("A1:A2"));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }

        private async Task FillAverageVisits(IEnumerable<AverageStudentVisitsDto> AverageStudentVisits)
        {
            if (AverageStudentVisits != null && AverageStudentVisits.Any())
            {
                xLWorkbook.AddWorksheet("Average visits of " + AverageStudentVisits.First().StudentGroup);
                var worksheet = xLWorkbook.Worksheet("Average visits of " + AverageStudentVisits.First().StudentGroup);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Student",
                    "Course",
                    "Student Group",
                    "Average visits percentage");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   AverageStudentVisits.First().Student);

                var orderedList = AverageStudentVisits.OrderBy(x => x.Student);
                for (int studentId = 0; studentId < orderedList.Count(); studentId++)
                {
                    var student = orderedList.ElementAt(studentId);

                    FillRow(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN - 1,
                        student.Course, student.StudentGroup, student.StudentAverageVisitsPercentage + " %");
                }

                DrawBorders(worksheet.Range(
                         worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN - 1),
                         worksheet.Row(orderedList.Count() + 1)
                         .Cell(_STUDENT_STARTING_COLUMN + 1)));

                DrawBorders(worksheet.Range("A1:A2"));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }

        public async Task FillFileAlternate(StudentsResultsDto data)
        {
            if (data.AverageStudentsMarks != null)
            {
                var worksheet = xLWorkbook.Worksheet(1);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course Id",
                    "Student Group Id",
                    "Student Id",
                    "Average mark");

                for (int row = 0; row < data.AverageStudentsMarks.Count(); row++)
                {
                    var xlRow = worksheet.Row(row + 2);
                    var mark = data.AverageStudentsMarks.ElementAt(row);

                    xlRow.Cell(1).Value = mark.Course;
                    xlRow.Cell(2).Value = mark.StudentGroup;
                    xlRow.Cell(3).Value = mark.Student;
                    xlRow.Cell(4).Value = mark.StudentAverageMark;
                }
            }

            if (data.AverageStudentVisits != null)
            {
                var worksheet = xLWorkbook.Worksheet(2);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course Id",
                    "Student Group Id",
                    "Student Id",
                    "Average visits percentage");

                for (int row = 0; row < data.AverageStudentVisits.Count(); row++)
                {
                    var xlRow = worksheet.Row(row + 2);
                    var presence = data.AverageStudentVisits.ElementAt(row);

                    xlRow.Cell(1).Value = presence.Course;
                    xlRow.Cell(2).Value = presence.StudentGroup;
                    xlRow.Cell(3).Value = presence.Student;
                    xlRow.Cell(4).Value = presence.StudentAverageVisitsPercentage;
                }
            }
        }
    }
}
