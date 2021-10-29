using CharlieBackend.Business.Services.FileServices.Importers;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportReaders
{
    public class XlsxFileReader : IFileReader
    {
        public Task<Result<IList<CreateThemeDto>>> ReadThemesAsync(
                string filePath)
        {
            var themes = new List<CreateThemeDto>();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    themes.Add(new CreateThemeDto
                    {
                        Name = row.Cell(1).GetValue<string>(),
                    });
                }
            }

            return Task.FromResult(Result<IList<CreateThemeDto>>.GetSuccess(themes));
        }

        Task<Result<IList<CreateAccountDto>>> IFileReader.ReadAccountsAsync(string filePath)
        {
            var accountsForStudents = new List<CreateAccountDto>();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    var account = new CreateAccountDto
                    {
                        Email = row
                        .Cell((int)StudentsGroupWorksheetHeader.StudentEmail)
                        .GetValue<string>(),

                        FirstName = row
                        .Cell((int)StudentsGroupWorksheetHeader.StudentFirstName)
                        .GetValue<string>(),

                        LastName = row
                        .Cell((int)StudentsGroupWorksheetHeader.StudentLastName)
                        .GetValue<string>(),
                    };

                    accountsForStudents.Add(account);
                }
            }

            return Task.FromResult(Result<IList<CreateAccountDto>>.GetSuccess(accountsForStudents));
        }
    }
}