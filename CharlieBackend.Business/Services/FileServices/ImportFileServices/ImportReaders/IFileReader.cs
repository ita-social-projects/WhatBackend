using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportReaders
{
    public interface IFileReader
    {
        Task<Result<IList<CreateAccountDto>>> ReadAccountsAsync(string filePath);

        Task<Result<IList<CreateThemeDto>>> ReadThemesAsync(string filePath);
    }
}
