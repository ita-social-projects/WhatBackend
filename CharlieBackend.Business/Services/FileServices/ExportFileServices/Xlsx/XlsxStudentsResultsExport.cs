using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class XlsxStudentsResultsExport : XlsxFileExport<StudentsResultsDto>
    {
        public XlsxStudentsResultsExport()
        {
            _xLWorkbook = new XLWorkbook();
        }

        public override string GetFileName()
        {
            return "StudentsResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
        }

        public override async Task FillFileAsync(StudentsResultsDto data)
        {
            if (data == null)
            {
                return;
            }

            if (data.AverageStudentsMarks != null && data.AverageStudentsMarks.Any())
            {
                var StudentsAverageMarks = data.AverageStudentsMarks.GroupBy(x => x.StudentGroup);
                foreach (var item in StudentsAverageMarks)
                {
                    await FillAverageMarksAsync(item
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
                    await FillAverageVisitsAsync(item
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

        private async Task FillAverageMarksAsync(IEnumerable<AverageStudentMarkDto> AverageStudentsMarks)
        {
            if (AverageStudentsMarks != null && AverageStudentsMarks.Any())
            {
                string worksheetName = "Average marks (" + (_xLWorkbook.Worksheets.Count + 1) + ")";
                _xLWorkbook.AddWorksheet(worksheetName);
                var worksheet = _xLWorkbook.Worksheet(worksheetName);

                await CreateHeadersAsync(worksheet.Row(1),
                   "Course",
                    "Student Group",
                    "Student",
                    "Average mark");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   AverageStudentsMarks.First().Course,
                   AverageStudentsMarks.First().StudentGroup);

                var orderedList = AverageStudentsMarks.OrderBy(x => x.Student);
                for (int studentId = 0; studentId < orderedList.Count(); studentId++)
                {
                    var student = orderedList.ElementAt(studentId);

                    FillRow(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN,
                        student.Student, Math.Round(((double)student.StudentAverageMark), 2)
                            .ToString(new NumberFormatInfo() 
                                { 
                                    NumberDecimalSeparator = "." 
                                }
                            ));
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

        private async Task FillAverageVisitsAsync(IEnumerable<AverageStudentVisitsDto> AverageStudentVisits)
        {
            if (AverageStudentVisits != null && AverageStudentVisits.Any())
            {
                string worksheetName = "Average visits (" + (_xLWorkbook.Worksheets.Count + 1) + ")";
                _xLWorkbook.AddWorksheet(worksheetName);
                var worksheet = _xLWorkbook.Worksheet(worksheetName);

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

                    FillRowWithNumberInLastColumnAsPercent(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN,
                        student.Student, ((double)student.StudentAverageVisitsPercentage / 100)
                        .ToString(new NumberFormatInfo()
                            {
                                NumberDecimalSeparator = "."
                            }
                        ));
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
