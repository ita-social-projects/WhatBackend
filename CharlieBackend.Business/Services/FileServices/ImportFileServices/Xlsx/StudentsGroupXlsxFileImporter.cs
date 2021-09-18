using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.FileServices.ImportFileServices.Base;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Models.ResultModel;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.Xlsx
{
    public class StudentsGroupXlsxFileImporter : StudentGroupFileImporter
    {
        private readonly IAccountService _accountService;

        public StudentsGroupXlsxFileImporter(
                IStudentGroupService studentGroupService,
                IStudentService studentService,
                IAccountService accountService)
                : base(studentService, studentGroupService)
        {
            _accountService = accountService;
        }

        protected override async Task<Result<IList<AccountDto>>> СollectAccountsFromFilePath(
                    string filePath)
        {
            var accountsForStudents = new List<AccountDto>();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    var account = new CreateAccountDto
                    {
                        Email = row.Cell((int)StudentsGroupWorksheetHeader.StudentEmail).GetValue<string>(),
                        FirstName = row.Cell((int)StudentsGroupWorksheetHeader.StudentFirstName).GetValue<string>(),
                        LastName = row.Cell((int)StudentsGroupWorksheetHeader.StudentLastName).GetValue<string>(),
                    };

                    if (await _accountService.IsEmailTakenAsync(account.Email))
                    {
                        var accountSearch = await _accountService
                                .GetAccountCredentialsByEmailAsync(account.Email);

                        accountsForStudents.Add(accountSearch.Data);
                    }
                    else
                    {
                        var _tempPassword = PasswordHelper.GeneratePassword();

                        account.Password = _tempPassword;
                        account.ConfirmPassword = _tempPassword;

                        var accountCreating = await _accountService
                            .CreateAccountAsync(account);

                        if (accountCreating.Error != null)
                        {
                            return Result<IList<AccountDto>>.GetError(
                                    accountCreating.Error.Code,
                                    accountCreating.Error.Message);
                        }
                        else
                        {
                            accountsForStudents.Add(accountCreating.Data);
                        }
                    }
                }
            }

            return Result<IList<AccountDto>>.GetSuccess(accountsForStudents);
        }
    }
}
