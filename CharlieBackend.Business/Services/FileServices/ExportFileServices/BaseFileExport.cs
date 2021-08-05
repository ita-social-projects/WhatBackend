using CharlieBackend.Core.Models.ResultModel;
using ClosedXML.Excel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class BaseFileExport
    {
        protected XLWorkbook xLWorkbook;
        protected MemoryStream memoryStream;

        protected const int _DEFAULT_STARTING_ROW = 2;
        protected const int _STUDENT_STARTING_ROW = 2;
        protected const int _DEFAULT_STARTING_COLUMN = 4;
        protected const int _STUDENT_STARTING_COLUMN = 3;

        public BaseFileExport()
        {
            xLWorkbook = new XLWorkbook();
            memoryStream = new MemoryStream();
        }

        public async Task<Result<IXLRow>> CreateHeadersAsync(IXLRow headerRow, params string[] argsList)
        {
            var result = await Task.Run(() => CreateHeaders(headerRow, argsList));

            return Result<IXLRow>.GetSuccess(result);
        }

        private IXLRow CreateHeaders(IXLRow headerRow, params string[] argsList)
        {
            for (int header = 0; header < argsList.Length; header++)
            {
                headerRow.Cell(header + 1).Value = argsList[header];
            }

            return headerRow;
        }

        public async Task<byte[]> GetByteArrayAsync()
        {
            return await Task.Run(()=> GetByteArray());
        }

        private byte[] GetByteArray()
        {
            if(xLWorkbook.Worksheets.Count == 0)
            {
                xLWorkbook.AddWorksheet("Blank");
            }
            xLWorkbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public virtual string GetFileName()
        {
            return "Filename_"+ DateTime.Now.ToString("yyyy-MM-dd") +".xlsx";
        }

        public string GetContentType()
        {
            return "application/octet-stream";
        }

        public void DrawBorders(IXLRange targetRange)
        {
            targetRange.Style
                .Border
                .OutsideBorder = XLBorderStyleValues.Medium;

            targetRange.Style
                .Border
                .InsideBorder = XLBorderStyleValues.Thin;
        }

        public void AdjustContent()
        {
            foreach (var worksheet in xLWorkbook.Worksheets)
            {
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
            
        }

        public void FillRow(IXLWorksheet worksheet, int rowNumber = 1, int startingColumnNumber = 1, params string[] argsList)
        {
            var currentRow = worksheet.Row(rowNumber);

            for (int cellId = 0; cellId < argsList.Length; cellId++)
            {
                currentRow.Cell(cellId + startingColumnNumber).Value = argsList[cellId];
            }
        }

        public void FillRowWithComments(IXLWorksheet worksheet, int rowNumber = 1, int startingColumnNumber = 1, params string[] argsList)
        {
            var currentRow = worksheet.Row(rowNumber);

            for (int cellId = 0; cellId < argsList.Length; cellId++)
            {
                if (argsList[cellId] != null)
                {
                    currentRow.Cell(cellId + startingColumnNumber).Comment.AddText(argsList[cellId]);
                }
            }
        }
    }
}
