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
        IOperatorImportProvider _provider;

        public ServiceImport(IOperatorImportProvider provider)
        {
            _provider = provider;
        }

        public async Task<Result<GroupWithStudentsDto>> ImportGroupAsync(
                IFormFile file, CreateStudentGroupDto group)
        {
            using FileService fileService = new FileService();

            var importOperator = GetImportOperator(file, fileService);

            if (importOperator.Error != null)
            {
                return Result<GroupWithStudentsDto>.GetError(
                        importOperator.Error.Code,
                        importOperator.Error.Message);
            }

            string filePath = await fileService.UploadFileAsync(file);

            var result = await importOperator.Data.ImportGroupAsync(group, filePath);

            return result;
        }

        public async Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(IFormFile file)
        {
            using FileService fileService = new FileService();
     
            var importOperator = GetImportOperator(file, fileService);

            if (importOperator.Error != null)
            {
                return Result<IEnumerable<ThemeDto>>.GetError(
                        importOperator.Error.Code,
                        importOperator.Error.Message);
            }

            var filePath = await fileService.UploadFileAsync(file);
            var themes = await importOperator.Data.ImportThemesAsync(filePath);

            return themes;
        }

        public Result<IOperatorImport> GetImportOperator(IFormFile file, 
            FileService fileService)
        {
            if (file == null)
            {
                return Result<IOperatorImport>.GetError(ErrorCode.ValidationError,
                            "File was not provided");
            }

            FileExtension extension = default;

            if (!fileService.IsFileExtensionValid(file, out extension))
            {
                return Result<IOperatorImport>.GetError(
                            ErrorCode.ValidationError,
                            "File extension not supported");
            }

            var importOperator = _provider.GetExportService(extension);

            if (importOperator == null)
            {
                return Result<IOperatorImport>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen");
            }
            else
            {
                return Result<IOperatorImport>.GetSuccess(importOperator);
            }
        }
    }
}
