using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.FileServices.ImportFileServices.Base;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.Csv
{
    public class StudentsGroupCsvFileImporter : StudentGroupFileImporter
    {
        private readonly IAccountService _accountService;

        public StudentsGroupCsvFileImporter(IAccountService accountService,
                IStudentGroupService studentGroupService,
                IStudentService studentService)
                : base(studentService, studentGroupService)
        {
            _accountService = accountService;
        }

        protected override async Task<Result<IList<AccountDto>>> СollectAccountsFromFilePath(
                 string filePath)
        {
            var accountsForStudents = new List<AccountDto>();

            using StreamReader stream = new StreamReader(filePath);

            string[] studentsInfo = null;

            while (stream.Peek() != -1)
            {
                sbyte count = 0;
                var line = stream.ReadLine().Trim('\n');

                studentsInfo = line.Split(';');

                var account = new CreateAccountDto()
                {
                    FirstName = studentsInfo[count++],
                    LastName = studentsInfo[count++],
                    Email = studentsInfo[count++],
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

            return Result<IList<AccountDto>>.GetSuccess(accountsForStudents);
        }
    }
}

