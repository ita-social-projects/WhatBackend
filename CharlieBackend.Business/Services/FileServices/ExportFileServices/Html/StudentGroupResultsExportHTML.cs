using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
	public class StudentGroupResultsExportHTML : BaseFileExportHTML
	{
		private string _fileName;
		private const int _CourseColumnNumber = 0;
		private const int _StudentGroupColumnNumber = 1;
		private const int _AverageMarkColumnNumber = 2;

		public StudentGroupResultsExportHTML(string fileName)
		{
			_fileName = fileName;
		}
		public void FillFileAsync(StudentGroupsResultsDto data)
		{
			if (data == null)
			{
				return;
			}
			FillAverageMarks(data.AverageStudentGroupsMarks);

			FillAverageVisits(data.AverageStudentGroupsVisits);
		}

		public void FillAverageMarks(IEnumerable<AverageStudentGroupMarkDto> AverageStudentGroupsMarks)
		{
			if (AverageStudentGroupsMarks.Any())
			{
				var orderedList = AverageStudentGroupsMarks.OrderBy(x => x.StudentGroup);
				string[] headers = new string[] { "Course", "Student Group", "Average mark" };
				string[][] rows = new string[orderedList.Count()][];

				for (int i = 0; i < rows.Length; i++)
				{
					var group = orderedList.ElementAt(i);
					rows[i] = new string[headers.Length];
					rows[i][_CourseColumnNumber] = AverageStudentGroupsMarks.First().Course;
					rows[i][_StudentGroupColumnNumber] = group.StudentGroup;
					rows[i][_AverageMarkColumnNumber] = Math.Round((decimal)group.AverageMark, 2).ToString();
				}

				StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);
				StringBuilder html = HtmlGenerator.GenerateHtml(_fileName, table);
				byte[] byteLine = ConvertLineToArray(html.ToString());
				_memoryStream.Write(byteLine);
			}
		}

		public void FillAverageVisits(IEnumerable<AverageStudentGroupVisitDto> AverageStudentGroupsVisits)
		{
			if (AverageStudentGroupsVisits.Any())
			{
				var orderedList = AverageStudentGroupsVisits.OrderBy(x => x.StudentGroup);
				string[] headers = new string[] { "Course", "Student Group", "Average visits" };
				string[][] rows = new string[orderedList.Count()][];

				for (int i = 0; i < rows.Length; i++)
				{
					var group = orderedList.ElementAt(i);
					rows[i] = new string[headers.Length];
					rows[i][_CourseColumnNumber] = AverageStudentGroupsVisits.First().Course;
					rows[i][_StudentGroupColumnNumber] = group.StudentGroup;
					rows[i][_AverageMarkColumnNumber] = ((double)group.AverageVisitPercentage / 100).ToString();
				}

				StringBuilder table = HtmlGenerator.GenerateTable(headers, rows);
				StringBuilder html = HtmlGenerator.GenerateHtml(_fileName, table);
				byte[] byteLine = ConvertLineToArray(html.ToString());
				_memoryStream.Write(byteLine);

			}
		}

		private byte[] ConvertLineToArray(string line)
		{
			byte[] array = new byte[line.Length];

			for (int i = 0; i < line.Length; i++)
			{
				array[i] = (byte)line[i];
			}

			return array;
		}

		public override string GetFileName()
		{
			return "StudentGroupResult_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html";
		}
	}
}
