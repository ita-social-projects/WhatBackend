using CharlieBackend.Business.Services.FileServices.Importers;
using CharlieBackend.Business.Services.FileServices.ImportFileServices.Importers;
using CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportReaders;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public class ServiceImport : IServiseImport
    {
        private readonly IFileReaderProvider _provider;
        private readonly IStudentService _studentService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IAccountService _accountService;
        private readonly IThemeService _themeService;

        public ServiceImport(IFileReaderProvider provider,
            IStudentService studentService,
            IStudentGroupService studentGroupService,
            IAccountService accountService,
            IThemeService themeService)
        {
            _provider = provider;
            _studentService = studentService;
            _studentGroupService = studentGroupService;
            _accountService = accountService;
            _themeService = themeService;
        }

        public async Task<Result<GroupWithStudentsDto>> ImportGroupAsync(
                IFormFile file, CreateStudentGroupDto group)
        {
            using FileService fileService = new FileService();

            var fileReader = GetImportReader(file, fileService);

            if (fileReader.Error != null)
            {
                return Result<GroupWithStudentsDto>.GetError(
                        fileReader.Error.Code,
                        fileReader.Error.Message);
            }

            string filePath = await fileService.UploadFileAsync(file);
            var accounts = await fileReader.Data.ReadAccountsAsync(filePath);

            var groupImporter = new StudentGroupImporter(_studentService,
                _studentGroupService, _accountService);

            await groupImporter.ImportGroupAsync(group, accounts.Data);

            return Result<GroupWithStudentsDto>.GetSuccess(new GroupWithStudentsDto());
        }

        public async Task<Result<IList<ThemeDto>>> ImportThemesAsync(IFormFile file)
        {
            using FileService fileService = new FileService();

            var importOperator = GetImportReader(file, fileService);

            if (importOperator.Error != null)
            {
                return Result<IList<ThemeDto>>.GetError(
                        importOperator.Error.Code,
                        importOperator.Error.Message);
            }

            var filePath = await fileService.UploadFileAsync(file);
            var themes = await importOperator.Data.ReadThemesAsync(filePath);

            var themeImporter = new ThemeImporter(_themeService);

            var themeResult = await themeImporter.ImportThemesAsync(themes.Data);

            return themeResult;
        }

        public Result<IFileReader> GetImportReader(IFormFile file,
            FileService fileService)
        {
            if (file == null)
            {
                return Result<IFileReader>.GetError(ErrorCode.ValidationError,
                            "File was not provided");
            }

            FileExtension extension = default;

            if (!fileService.IsFileExtensionValid(file, out extension))
            {
                return Result<IFileReader>.GetError(
                            ErrorCode.ValidationError,
                            "File extension not supported");
            }

            var importReader = _provider.GetFileReader(extension);

            if (importReader == null)
            {
                return Result<IFileReader>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen");
            }
            else
            {
                return Result<IFileReader>.GetSuccess(importReader);
            }
        }
    }
}