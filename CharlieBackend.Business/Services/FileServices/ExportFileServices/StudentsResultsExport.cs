using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class StudentsResultsExport : BaseFileExport
    {
        public StudentsResultsExport()
        {
            xLWorkbook = new XLWorkbook();
        }

        public override string GetFileName()
        {
            return "StudentResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
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

        public async Task FillFile(StudentsResultsDto data)
        {
            if (data.AverageStudentsMarks != null && data.AverageStudentsMarks.Any())
            {
                var StudentsAverageMarks = data.AverageStudentsMarks.GroupBy(x => x.StudentGroup);
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
                var StudentsPresences = data.AverageStudentVisits.GroupBy(x => x.StudentGroup);
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
                xLWorkbook.AddWorksheet("Average marks of " + AverageStudentsMarks.First().StudentGroup);
                var worksheet = xLWorkbook.Worksheet("Average marks of " + AverageStudentsMarks.First().StudentGroup);

                await CreateHeadersAsync(worksheet.Row(1),
                   "Course",
                    "Student Group",
                    "Student",
                    "Average mark percentage");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   AverageStudentsMarks.First().Course,
                   AverageStudentsMarks.First().StudentGroup);

                var orderedList = AverageStudentsMarks.OrderBy(x => x.Student);
                for (int studentId = 0; studentId < orderedList.Count(); studentId++)
                {
                    var student = orderedList.ElementAt(studentId);

                    FillRow(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN,
                        student.Student, Math.Round(((decimal)student.StudentAverageMark), 2).ToString());
                }

                DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN),
                        worksheet.Row(orderedList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN + 1)));

                DrawBorders(worksheet.Range("A1:B2"));

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
                   "Course",
                    "Student Group",
                    "Student",
                    "Average visits percentage");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   AverageStudentVisits.First().Course,
                   AverageStudentVisits.First().StudentGroup);

                var orderedList = AverageStudentVisits.OrderBy(x => x.Student);
                for (int studentId = 0; studentId < orderedList.Count(); studentId++)
                {
                    var student = orderedList.ElementAt(studentId);

                    FillRow(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN,
                        student.Student, student.StudentAverageVisitsPercentage.ToString() + " %");
                }

                DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN),
                        worksheet.Row(orderedList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN + 1)));

                DrawBorders(worksheet.Range("A1:B2"));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }
    }
}
