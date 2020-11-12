using System;
using AutoMapper;
using ClosedXML.Excel;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CharlieBackend.Business.Services
{
    public class ImportService : IImportService
    {
        #region private
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        public ImportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<StudentGroup>>> ImportFileAsync(ImportFileDto file)
        {
            List<StudentGroup> importedGroups = new List<StudentGroup>();
            var wb = new XLWorkbook(file.url);
            var wsGroups = wb.Worksheet("Groups");

            Type sgType = typeof(StudentGroupFileModel);
            char charPointer = 'A';
            int numPointer = 2;

            var properties = sgType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != Convert.ToString(wsGroups.Cell($"{charPointer}1").Value))
                {
                    return Result<List<StudentGroup>>.Error(ErrorCode.ValidationError,
                                "The format of the downloaded file is not suitable."
                                     + " Check headers in the file");
                }
                charPointer++;
            }
        
            StudentGroupFileModel fileLine = new StudentGroupFileModel();
            while (!await IsEndOfFileAsync(numPointer, wsGroups))
            {
                try
                {
                    fileLine.CourseId = Convert
                        .ToString(wsGroups.Cell($"B{numPointer}").Value);
                    fileLine.Name = Convert
                        .ToString(wsGroups.Cell($"C{numPointer}").Value);
                    fileLine.StartDate = Convert
                        .ToDateTime(wsGroups.Cell($"D{numPointer}").Value);
                    fileLine.FinishDate = Convert
                        .ToDateTime(wsGroups.Cell($"E{numPointer}").Value);

                    await IsValueValid(fileLine, numPointer);

                    StudentGroup group = new StudentGroup
                    {
                        CourseId = Convert.ToInt32(fileLine.CourseId),
                        Name = fileLine.Name,
                        StartDate = fileLine.StartDate,
                        FinishDate = fileLine.FinishDate,
                    };

                    importedGroups.Add(group);
                    _unitOfWork.StudentGroupRepository.Add(group);
                    numPointer++;
                }
                catch (FormatException ex)
                {
                    _unitOfWork.Rollback();
                    return Result<List<StudentGroup>>.Error(ErrorCode.ValidationError,
                        "The format of the inputed data is incorrect.\n" + ex.Message);
                }
                try 
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    _unitOfWork.Rollback();
                    return Result<List<StudentGroup>>.Error(ErrorCode.ValidationError,
                        "The format of the inputed data is incorrect.\n " +
                        "Such value of Name field already exists.\n" +
                        $"Problem was occured in col C, row { numPointer}");
                }

            }
            return Result<List<StudentGroup>>
                .Success(_mapper.Map<List<StudentGroup>>(importedGroups));
        }

        private async Task IsValueValid(StudentGroupFileModel fileLine, int numPointer)
        {
            if (fileLine.CourseId.Replace(" ", "") == "")
            {
                throw new FormatException("CourseId field shouldn't be empty.\n" +
                    $"Problem was occured in col B, row {numPointer}");
            }

            if (fileLine.Name == "")
            {
                throw new FormatException("Name field shouldn't be empty.\n" +
                    $"Problem was occured in col C, row {numPointer}");
            }

            if (fileLine.StartDate > fileLine.FinishDate)
            {
                throw new FormatException("StartDate must be less than FinishDate.\n" +
                    $"Problem was occured in col D/E, row {numPointer}");
            }
        }

        private async Task<bool> IsEndOfFileAsync(int counter, IXLWorksheet ws)
        {
            if (Convert.ToString(ws.Cell($"B{counter}").Value) == ""
               && Convert.ToString(ws.Cell($"C{counter}").Value) == ""
               && Convert.ToString(ws.Cell($"D{counter}").Value) == ""
               && Convert.ToString(ws.Cell($"E{counter}").Value) == "")
            {
                return true;
            }
            return false;
        }
    }
}
