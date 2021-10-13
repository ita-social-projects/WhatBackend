using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportReaders
{
    public class CsvFileReader : IFileReader
    {
        public Task<Result<IList<CreateAccountDto>>> ReadAccountsAsync(string filePath)
        {
            using StreamReader stream = new StreamReader(filePath);

            var accountsForStudents = new List<CreateAccountDto>();
            string[] studentsInfo = null;

            while (stream.Peek() != -1)
            {
                sbyte count = 0;
                var line = stream.ReadLine().Trim('\n');

                studentsInfo = line.Split(';');

                accountsForStudents.Add(new CreateAccountDto()
                {
                    FirstName = studentsInfo[count++],
                    LastName = studentsInfo[count++],
                    Email = studentsInfo[count++],
                });
            }

            return Task.FromResult(Result<IList<CreateAccountDto>>.GetSuccess(
                    accountsForStudents));
        }

        public Task<Result<IList<CreateThemeDto>>> ReadThemesAsync(string filePath)
        {
            var themes = new List<CreateThemeDto>();

            using StreamReader stream = new StreamReader(filePath);

            string[] themeInfo = null;

            while (stream.Peek() != -1)
            {
                themeInfo = stream.ReadLine().Trim('\n').Split(';');
          
                themes.Add(new CreateThemeDto() { Name = themeInfo[0] });
            }

            return Task.FromResult(Result<IList<CreateThemeDto>>.GetSuccess(themes));
        }
    }
}
