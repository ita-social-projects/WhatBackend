using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Export;

namespace CharlieBackend.Business.Services
{
    public class ExportServiceXlsx : IExportService
    {
        public async Task<FileDto> GetStudentsClassbook(StudentsClassbookResultDto data)
        {
            using var classbook = new ClassbookExportXlsx();

            await classbook.FillFile(data);
            classbook.AdjustContent();

            return new FileDto
            {
                ByteArray = await classbook.GetByteArrayAsync(),
                ContentType = classbook.GetContentType(),
                Filename = classbook.GetFileName()
            };
        }

        public async Task<FileDto> GetStudentsResults(StudentsResultsDto data)
        {
            using var result = new StudentsResultsExportXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            };
        }

        public async Task<FileDto> GetStudentClassbook(StudentsClassbookResultDto data)
        {
            using var result = new StudentClassbookXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            };
        }

        public async Task<FileDto> GetStudentResults(StudentsResultsDto data)
        {
            using var result = new StudentResultXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            };
        }

        public async Task<FileDto> GetStudentGroupResults(StudentGroupsResultsDto data)
        {
            using var result = new StudentGroupResultsXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            };
        }
    }
}
