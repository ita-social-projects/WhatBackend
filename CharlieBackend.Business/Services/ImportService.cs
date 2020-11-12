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

        public async Task<Result<List<StudentGroupFileModel>>> ImportFileAsync(ImportFileDto file)
        {
            var wb = new XLWorkbook(file.url);
            var wsGroups = wb.Worksheet("Groups");
            List<StudentGroupFileModel> importedGroups = new List<StudentGroupFileModel>();

            Type sgType = typeof(StudentGroupFileModel);
            char charPointer = 'A';
            int numPointer = 2;

            var properties = sgType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != Convert.ToString(wsGroups.Cell($"{charPointer}1").Value))
                {
                    return Result<List<StudentGroupFileModel>>.Error(ErrorCode.ValidationError,
                                "The format of the downloaded file is not suitable."
                                     + "Check headers in the file.");
                }
                charPointer++;
            }
        
            while (!await IsEndOfFileAsync(numPointer, wsGroups))
            {

                try
                {
                StudentGroupFileModel fileLine = new StudentGroupFileModel();

                    fileLine.CourseId = wsGroups.Cell($"B{numPointer}").Value.ToString();
                    fileLine.Name = wsGroups.Cell($"C{numPointer}").Value.ToString();
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

                    importedGroups.Add(fileLine);
                    _unitOfWork.StudentGroupRepository.Add(group);
                    numPointer++;
                }
                catch (FormatException ex)
                {
                    _unitOfWork.Rollback();
                    return Result<List<StudentGroupFileModel>>.Error(ErrorCode.ValidationError,
                        "The format of the inputed data is incorrect.\n" + ex.Message);
                }
                catch (DbUpdateException ex)
                {
                    _unitOfWork.Rollback();
                    return Result<List<StudentGroupFileModel>>.Error(ErrorCode.ValidationError,
                        "Inputed data is incorrect.\n" + ex.Message);
                }
            }
            
            await _unitOfWork.CommitAsync();
            return Result<List<StudentGroupFileModel>>
                .Success(_mapper.Map<List<StudentGroupFileModel>>(importedGroups));
        }

        private async Task IsValueValid(StudentGroupFileModel fileLine, int numPointer)
        {
            List<long> existingCourseIds = new List<long>();
            List<string> existingGroupNames = new List<string>();

            foreach (Course course in await _unitOfWork.CourseRepository.GetAllAsync())
            {
                existingCourseIds.Add(course.Id);
            }

            foreach (StudentGroup group in await _unitOfWork.StudentGroupRepository.GetAllAsync())
            {
                existingGroupNames.Add(group.Name);
            }


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
                    $"Problem was occured in col D/E, row {numPointer}.");
            }

            if (!existingCourseIds.Contains(Convert.ToInt64(fileLine.CourseId)))
            {
                throw new DbUpdateException($"Course with id {fileLine.CourseId} doesn't exist.\n" +
                   $"Problem was occured in col B, row {numPointer}.");
            }

            if (existingGroupNames.Contains(fileLine.Name))
            {
                throw new DbUpdateException($"Group with name {fileLine.Name} already exist.\n" +
                   $"Problem was occured in col C, row {numPointer}.");
            }
        }

        private async Task<bool> IsEndOfFileAsync(int counter, IXLWorksheet ws)
        { 
            return (ws.Cell($"B{counter}").Value.ToString() == "")
               && (ws.Cell($"C{counter}").Value.ToString() == "")
               && (ws.Cell($"D{counter}").Value.ToString() == "")
               && (ws.Cell($"E{counter}").Value.ToString() == "");
        }
    }
}
