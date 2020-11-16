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
    public class FileImportService : IFileImportService
    {
        #region private
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        public FileImportService(IUnitOfWork unitOfWork, IMapper mapper)
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
            int rowCounter = 2;

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

            while (!IsEndOfFile(rowCounter, wsGroups))
            {

                try
                {
                    StudentGroupFileModel fileLine = new StudentGroupFileModel();

                    fileLine.CourseId = wsGroups.Cell($"B{rowCounter}").Value.ToString();
                    fileLine.Name = wsGroups.Cell($"C{rowCounter}").Value.ToString();
                    fileLine.StartDate = Convert
                        .ToDateTime(wsGroups.Cell($"D{rowCounter}").Value);
                    fileLine.FinishDate = Convert
                        .ToDateTime(wsGroups.Cell($"E{rowCounter}").Value);

                    await IsValueValid(fileLine, rowCounter);

                    StudentGroup group = new StudentGroup
                    {
                        CourseId = Convert.ToInt32(fileLine.CourseId),
                        Name = fileLine.Name,
                        StartDate = fileLine.StartDate,
                        FinishDate = fileLine.FinishDate,
                    };

                    importedGroups.Add(fileLine);
                    _unitOfWork.StudentGroupRepository.Add(group);
                    rowCounter++;
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

        private async Task IsValueValid(StudentGroupFileModel fileLine, int rowCounter)
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
                    $"Problem was occured in col B, row {rowCounter}");
            }

            if (fileLine.Name == "")
            {
                throw new FormatException("Name field shouldn't be empty.\n" +
                    $"Problem was occured in col C, row {rowCounter}");
            }

            if (fileLine.StartDate > fileLine.FinishDate)
            {
                throw new FormatException("StartDate must be less than FinishDate.\n" +
                    $"Problem was occured in col D/E, row {rowCounter}.");
            }

            if (!existingCourseIds.Contains(Convert.ToInt64(fileLine.CourseId)))
            {
                throw new DbUpdateException($"Course with id {fileLine.CourseId} doesn't exist.\n" +
                   $"Problem was occured in col B, row {rowCounter}.");
            }

            if (existingGroupNames.Contains(fileLine.Name))
            {
                throw new DbUpdateException($"Group with name {fileLine.Name} already exist.\n" +
                   $"Problem was occured in col C, row {rowCounter}.");
            }
        }

        private bool IsEndOfFile(int rowCounter, IXLWorksheet ws)
        {
            return (ws.Cell($"B{rowCounter}").Value.ToString() == "")
               && (ws.Cell($"C{rowCounter}").Value.ToString() == "")
               && (ws.Cell($"D{rowCounter}").Value.ToString() == "")
               && (ws.Cell($"E{rowCounter}").Value.ToString() == "");
        }

    }
}
