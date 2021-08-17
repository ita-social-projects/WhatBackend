using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class StudentClassbook : BaseFileExport
    {
        public StudentClassbook()
        {
            xLWorkbook = new XLWorkbook();
        }

        public override string GetFileName()
        {
            return "StudentsClassbookResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
        }

        public async Task FillFile(StudentsClassbookResultDto data)
        {
            if (data.StudentsMarks != null && data.StudentsMarks.Any())
            {
                var StudentsMarks = data.StudentsMarks.GroupBy(x => x.Student);
                foreach (var item in StudentsMarks)
                {
                    await FillStudentAverageMark(item
                        .Select(x => new StudentMarkDto
                        {
                            StudentMark = x.StudentMark,
                            Course = x.Course,
                            Student = x.Student,
                            StudentGroup = x.StudentGroup,
                            StudentId = x.StudentId,
                            Comment = x.Comment,
                            LessonDate = x.LessonDate,
                            LessonId = x.LessonId
                        }
                        ));
                }
            }

            if (data.StudentsPresences != null && data.StudentsPresences.Any())
            {
                var StudentVisit = data.StudentsPresences.GroupBy(x => x.Student);
                foreach (var item in StudentVisit)
                {
                    await FillStudentPresence(item
                        .Select(x => new StudentVisitDto
                        {
                            LessonDate = x.LessonDate,
                            Course = x.Course,
                            Student = x.Student,
                            StudentGroup = x.StudentGroup,
                            LessonId = x.LessonId,
                            Presence = x.Presence,
                            StudentId = x.StudentId
                        }
                        ));
                }
            }
        }

        public async Task FillStudentPresence(IEnumerable<StudentVisitDto> StudentVisit)
        {
            xLWorkbook.AddWorksheet("Presence of " + StudentVisit.First().Student);
            var worksheet = xLWorkbook.Worksheet("Presence of " + StudentVisit.First().Student);

            await CreateHeadersAsync(worksheet.Row(1),
                "Student",
                "Course",
                "Student Group",
                "Lesson date",
                "Presence");

            FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   StudentVisit.First().Student);

            var orderedList = StudentVisit.OrderBy(x => x.Course).ThenBy(x => x.StudentGroup).ThenBy(x => x.LessonDate);
            for (int studentId = 0; studentId < orderedList.Count(); studentId++)
            {
                var student = orderedList.ElementAt(studentId);

                FillRow(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN - 1,
                    student.Course, student.StudentGroup, student.LessonDate.ToString(), 
                    student.Presence == true ? "+" : student.Presence == false ? "-" : " ");

            }

            DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN - 1),
                        worksheet.Row(orderedList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN + 2)));

            DrawBorders(worksheet.Range("A1:A2"));

            worksheet.Columns().AdjustToContents();
            worksheet.Rows().AdjustToContents();
        }

        public async Task FillStudentAverageMark(IEnumerable<StudentMarkDto> StudentsMarks)
        {
            xLWorkbook.AddWorksheet("Average mark of " + StudentsMarks.First().Student);
            var worksheet = xLWorkbook.Worksheet("Average mark of " + StudentsMarks.First().Student);

            await CreateHeadersAsync(worksheet.Row(1),
                "Student",
                "Course",
                "Student Group",
                "Lesson date",
                "Average mark");

            FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   StudentsMarks.First().Student);

            var orderedList = StudentsMarks.OrderBy(x => x.Course).ThenBy(x => x.StudentGroup).ThenBy(x => x.LessonDate);
            for (int studentId = 0; studentId < orderedList.Count(); studentId++)
            {
                var student = orderedList.ElementAt(studentId);

                FillRow(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN - 1,
                    student.Course, student.StudentGroup, student.LessonDate.ToString(), student.StudentMark.ToString());

                FillRowWithComments(worksheet, studentId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN + 3,
                    student.Comment);
            }

            DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN - 1),
                        worksheet.Row(orderedList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN + 2)));

            DrawBorders(worksheet.Range("A1:A2"));

            worksheet.Columns().AdjustToContents();
            worksheet.Rows().AdjustToContents();
        }
    }
}
