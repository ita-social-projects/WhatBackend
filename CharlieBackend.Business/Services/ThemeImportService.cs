using System;
using System.IO;
using AutoMapper;
using ClosedXML.Excel;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Business.Services
{
    public class ThemeImportService : IThemeImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ThemeImportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ThemeFile>>> ImportFileAsync(IFormFile uploadedFile)
        {
            string path = "";
            List<ThemeFile> importedThemes = new List<ThemeFile>();

            if (uploadedFile != null)
            {
                path = await CreateFile(uploadedFile);
                var book = new XLWorkbook(path);
                var themesSheet = book.Worksheet("Themes");

                char charPointer = 'A';
                int rowCounter = 2;

                var properties = typeof(ThemeFile).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name != Convert.ToString(themesSheet.Cell($"{charPointer}1").Value))
                    {
                        return Result<List<ThemeFile>>.GetError(ErrorCode.ValidationError,
                                    "The format of the downloaded file is not suitable."
                                         + "Check headers in the file.");
                    }
                    charPointer++;
                }

                while (!IsEndOfFile(rowCounter, themesSheet))
                {
                    try
                    {
                        ThemeFile fileLine = new ThemeFile
                        {
                            Theme = themesSheet.Cell($"A{rowCounter}").Value.ToString(),
                        };

                        //await IsValueValid(fileLine, rowCounter);

                        Theme theme = new Theme
                        {
                            Name = fileLine.Theme,
                        };

                        importedThemes.Add(fileLine);
                        _unitOfWork.ThemeRepository.Add(theme);
                        rowCounter++;
                    }
                    catch (FormatException ex)
                    {
                        _unitOfWork.Rollback();

                        return Result<List<ThemeFile>>.GetError(ErrorCode.ValidationError,
                            "The format of the inputed data is incorrect.\n" + ex.Message);
                    }
                    catch (DbUpdateException ex)
                    {
                        _unitOfWork.Rollback();

                        return Result<List<ThemeFile>>
                            .GetError(ErrorCode.ValidationError,
                                "Inputed data is incorrect.\n" + ex.Message);
                    }
                }
            }
            await _unitOfWork.CommitAsync();

            Array.ForEach(Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files")), File.Delete);

            return Result<List<ThemeFile>>
                .GetSuccess(_mapper.Map<List<ThemeFile>>(importedThemes));
        }

        /*private async Task IsValueValid(ThemeFile fileLine, int rowCounter)
        {
        }*/

        private bool IsEndOfFile(int rowCounter, IXLWorksheet sheet)
        {
            return (sheet.Cell($"A{rowCounter}").Value.ToString() == "");
        }

        private async Task<string> CreateFile(IFormFile file)
        {
            string path = "";
            string fileName;
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return path;
        }

        public bool CheckIfExcelFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

            return (extension == ".xlsx" || extension == ".xls");
        }
    }
}
