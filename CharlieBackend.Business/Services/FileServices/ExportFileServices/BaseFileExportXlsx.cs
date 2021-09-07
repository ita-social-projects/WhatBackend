using CharlieBackend.Core.Models.ResultModel;
using ClosedXML.Excel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class BaseFileExportXlsx
    {
        protected XLWorkbook xLWorkbook;
        protected MemoryStream memoryStream;

        protected const int _DEFAULT_STARTING_ROW = 2;
        protected const int _STUDENT_STARTING_ROW = 2;
        protected const int _DEFAULT_STARTING_COLUMN = 4;
        protected const int _STUDENT_STARTING_COLUMN = 3;

        public BaseFileExportXlsx()
        {
            xLWorkbook = new XLWorkbook();
            memoryStream = new MemoryStream();
        }

        private IXLRow CreateHeaders(IXLRow headerRow, params string[] argsList)
        {
            for (int header = 0; header < argsList.Length; header++)
            {
                headerRow.Cell(header + 1).Value = argsList[header];
            }

            return headerRow;
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

        /// <summary>
        /// Fills row with headers starting from first column (can be used for other rows as well)
        /// </summary>
        /// <param name="headerRow">Row which has to be filled</param>
        /// <param name="argsList">Variable number of params which will be inserted into mentioned row</param>
        /// <returns>Ref to the row has been filled</returns>
        public async Task<Result<IXLRow>> CreateHeadersAsync(IXLRow headerRow, params string[] argsList)
        {
            var result = await Task.Run(() => CreateHeaders(headerRow, argsList));

            return Result<IXLRow>.GetSuccess(result);
        }

        /// <summary>
        /// Creates byte arrray from file (Should be used to create and return file)
        /// </summary>
        /// <returns>Byte arrray</returns>
        public async Task<byte[]> GetByteArrayAsync()
        {
            return await Task.Run(()=> GetByteArray());
        }

        /// <summary>
        /// Returns file name and has to be overrided by inheritors to set specified file name
        /// </summary>
        /// <returns>
        /// (String) file name
        /// </returns>
        public virtual string GetFileName()
        {
            return "Filename_"+ DateTime.Now.ToString("yyyy-MM-dd") +".xlsx";
        }

        /// <summary>
        /// Returns content type (Should be used to create and return file)
        /// </summary>
        /// <returns>
        /// application/octet-stream
        /// </returns>
        public string GetContentType()
        {
            return "application/octet-stream";
        }

        /// <summary>
        /// Draws borders for specified range (Medium outer borders and thin inner borders)
        /// </summary>
        /// <param name="targetRange">Range which needs to get borders</param>
        public void DrawBorders(IXLRange targetRange)
        {
            targetRange.Style
                .Border
                .OutsideBorder = XLBorderStyleValues.Medium;

            targetRange.Style
                .Border
                .InsideBorder = XLBorderStyleValues.Thin;
        }

        /// <summary>
        /// Ajusts content for all sheets in excel book 
        /// (Sets width and height to avoid hiding part of words etc.)
        /// </summary>
        public void AdjustContent()
        {
            foreach (var worksheet in xLWorkbook.Worksheets)
            {
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
            }
            
        }

        /// <summary>
        /// Fills rows with parameters in argList
        /// </summary>
        /// <param name="worksheet"> Worksheet which has to be filled </param>
        /// <param name="rowNumber"> id of row which has to be filled </param>
        /// <param name="startingColumnNumber"> id of column from where cells would be filled </param>
        /// <param name="argsList"> 
        /// List of parameters which will be inserted 
        /// If you need to skip some cells use "" or " "
        /// </param>
        public void FillRow(IXLWorksheet worksheet, int rowNumber = 1, int startingColumnNumber = 1,
            params string[] argsList)
        {
            var currentRow = worksheet.Row(rowNumber);

            for (int cellId = 0; cellId < argsList.Length; cellId++)
            {
                currentRow.Cell(cellId + startingColumnNumber).Value = argsList[cellId];
            }
        }

        /// <summary>
        /// Fills rows with parameters in argList as numbers
        /// </summary>
        /// <param name="worksheet"> Worksheet which has to be filled </param>
        /// <param name="rowNumber"> id of row which has to be filled </param>
        /// <param name="startingColumnNumber"> id of column from where cells would be filled </param>
        /// <param name="argsList"> 
        /// List of parameters which will be inserted 
        /// If you need to skip some cells use "" or " "
        /// </param>
        public void FillRowWithNumberInLastColumnAsPercent(IXLWorksheet worksheet, int rowNumber = 1, int startingColumnNumber = 1,
            params string[] argsList)
        {
            var currentRow = worksheet.Row(rowNumber);

            for (int cellId = 0; cellId < argsList.Length; cellId++)
            {
                currentRow.Cell(cellId + startingColumnNumber).Value = argsList[cellId];
            }

            currentRow.Cell(startingColumnNumber + argsList.Length - 1).Style.NumberFormat.Format = "0%";
        }

        /// <summary>
        /// Fills rows with parameters in argList in the cells centers
        /// </summary>
        /// <param name="worksheet"> Worksheet which has to be filled </param>
        /// <param name="rowNumber"> id of row which has to be filled </param>
        /// <param name="startingColumnNumber"> id of column from where cells would be filled </param>
        /// <param name="argsList"> 
        /// List of parameters which will be inserted 
        /// If you need to skip some cells use "" or " "
        /// </param>
        public void FillRowTextAlignCenter(IXLWorksheet worksheet, int rowNumber = 1, int startingColumnNumber = 1,
            params string[] argsList)
        {
            var currentRow = worksheet.Row(rowNumber);

            for (int cellId = 0; cellId < argsList.Length; cellId++)
            {
                currentRow.Cell(cellId + startingColumnNumber).Value = argsList[cellId];
                currentRow.Cell(cellId + startingColumnNumber).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }
        }

        /// <summary>
        /// Fills rows with parameters in argList as comments
        /// </summary>
        /// <param name="worksheet"> Worksheet which has to be filled </param>
        /// <param name="rowNumber"> id of row which has to be filled </param>
        /// <param name="startingColumnNumber"> id of column from where cells would be filled </param>
        /// <param name="argsList"> 
        /// List of parameters which will be inserted 
        /// nulls will be ignored
        /// If you want to avoid empty comments replace all "" with null before calling this method
        /// </param>
        public void FillRowWithComments(IXLWorksheet worksheet, int rowNumber = 1,
            int startingColumnNumber = 1, params string[] argsList)
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
