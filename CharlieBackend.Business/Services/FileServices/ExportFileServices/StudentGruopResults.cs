using CharlieBackend.Core.DTO.Dashboard;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class StudentGruopResults : BaseFileExport
    {
        public StudentGruopResults()
        {
            xLWorkbook = new XLWorkbook();
        }

        public override string GetFileName()
        {
            return "StudentResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
        }

        public async Task FillFile(StudentGroupsResultsDto data)
        {
            if (data.AverageStudentGroupsMarks != null && data.AverageStudentGroupsMarks.Any())
            {
                await FillAverageMarks(data.AverageStudentGroupsMarks);
            }

            if (data.AverageStudentGroupsVisits != null && data.AverageStudentGroupsVisits.Any())
            {
                await FillAverageVisits(data.AverageStudentGroupsVisits);
            }
        }

        public async Task FillAverageMarks(IEnumerable<AverageStudentGroupMarkDto> AverageStudentGroupsMarks)
        {
            if (AverageStudentGroupsMarks != null && AverageStudentGroupsMarks.Any())
            {
                xLWorkbook.AddWorksheet("Average mark of " + AverageStudentGroupsMarks.First().Course);
                var worksheet = xLWorkbook.Worksheet("Average mark of " + AverageStudentGroupsMarks.First().Course);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Average mark");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   AverageStudentGroupsMarks.First().Course);

                var orderedList = AverageStudentGroupsMarks.OrderBy(x => x.StudentGroup);
                for (int groupId = 0; groupId < orderedList.Count(); groupId++)
                {
                    var group = orderedList.ElementAt(groupId);

                    FillRow(worksheet, groupId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN - 1,
                        group.StudentGroup,  Math.Round(((decimal)group.AverageMark), 2).ToString());
                }

                DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN - 1),
                        worksheet.Row(orderedList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN)));

                DrawBorders(worksheet.Range("A1:A2"));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }

        public async Task FillAverageVisits(IEnumerable<AverageStudentGroupVisitDto> AverageStudentGroupsVisits)
        {
            if (AverageStudentGroupsVisits != null && AverageStudentGroupsVisits.Any())
            {
                xLWorkbook.AddWorksheet("Average visits of " + AverageStudentGroupsVisits.First().Course);
                var worksheet = xLWorkbook.Worksheet("Average visits of " + AverageStudentGroupsVisits.First().Course);

                await CreateHeadersAsync(worksheet.Row(1),
                    "Course",
                    "Student Group",
                    "Average visits");

                FillRow(worksheet, _DEFAULT_STARTING_ROW, 1,
                   AverageStudentGroupsVisits.First().Course);

                var orderedList = AverageStudentGroupsVisits.OrderBy(x => x.StudentGroup);
                for (int groupId = 0; groupId < orderedList.Count(); groupId++)
                {
                    var group = orderedList.ElementAt(groupId);

                    FillRow(worksheet, groupId + _STUDENT_STARTING_ROW, _STUDENT_STARTING_COLUMN - 1,
                        group.StudentGroup, group.AverageVisitPercentage + " %");
                }

                DrawBorders(worksheet.Range(
                        worksheet.Row(1).Cell(_STUDENT_STARTING_COLUMN - 1),
                        worksheet.Row(orderedList.Count() + 1)
                        .Cell(_STUDENT_STARTING_COLUMN)));

                DrawBorders(worksheet.Range("A1:A2"));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
        }
    }
}
