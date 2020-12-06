﻿using System;
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
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Business.Services
{
    public class StudentImportService : IStudentImportService
    {
        #region private
        private readonly IStudentService _studentService;
        private readonly IStudentGroupService _studentGroupService; 
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        public StudentImportService(IMapper mapper,
                                    IUnitOfWork unitOfWork,
                                    IAccountService accountService,
                                    IStudentService studentService,
                                    IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
            _studentService = studentService;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<StudentFileModel>>> ImportFileAsync(long groupId, IFormFile uploadedFile)
        {
            List<StudentFileModel> importedAccounts = new List<StudentFileModel>();
            var groupName = _unitOfWork.StudentGroupRepository.SearchStudentGroup(groupId).Name;
            if (!await _unitOfWork.StudentGroupRepository.IsGroupNameExistAsync(groupName))
            {
                return Result<List<StudentFileModel>>.GetError(ErrorCode.NotFound, $"Group {groupName} doesn't exist.");
            }

            if (uploadedFile != null)
            {
                var book = new XLWorkbook(await CreateFile(uploadedFile));
                var studentsSheet = book.Worksheet("Students");

                Type studentType = typeof(StudentFileModel);
                char charPointer = 'A';
                int rowCounter = 2;

                var properties = studentType.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name != Convert.ToString(studentsSheet.Cell($"{charPointer}1").Value))
                    {
                        return Result<List<StudentFileModel>>.GetError(ErrorCode.ValidationError,
                                    "The format of the downloaded file is not suitable."
                                         + "Check headers in the file.");
                    }
                    charPointer++;
                }

                while (!IsEndOfFile(rowCounter, studentsSheet))
                {

                    try
                    {
                        StudentFileModel fileLine = new StudentFileModel
                        {
                            Email = studentsSheet.Cell($"A{rowCounter}").Value.ToString(),
                            FirstName = studentsSheet.Cell($"B{rowCounter}").Value.ToString(),
                            LastName = studentsSheet.Cell($"C{rowCounter}").Value.ToString()
                        };

                        await IsValueValid(fileLine, rowCounter);

                        CreateAccountDto studentAccount = new CreateAccountDto
                        {
                            Email = fileLine.Email,
                            FirstName = fileLine.FirstName,
                            LastName = fileLine.LastName,
                            Password = "changeYourPassword",
                            ConfirmPassword = "changeYourPassword"
                        };

                        await _accountService.CreateAccountAsync(studentAccount);

                        importedAccounts.Add(fileLine);
                        rowCounter++;
                    }
                    catch (FormatException ex)
                    {
                        _unitOfWork.Rollback();

                        return Result<List<StudentFileModel>>.GetError(ErrorCode.ValidationError,
                            "The format of the inputed data is incorrect.\n" + ex.Message);
                    }
                    catch (DbUpdateException ex)
                    {
                        _unitOfWork.Rollback();

                        return Result<List<StudentFileModel>>.GetError(ErrorCode.ValidationError,
                            "Inputed data is incorrect.\n" + ex.Message);
                    }
                }
            }
            await _unitOfWork.CommitAsync();

            Array.ForEach(Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files")), File.Delete);

            await BoundStudentsToTheGroupAsync(importedAccounts, groupId);

            return Result<List<StudentFileModel>>
                .GetSuccess(_mapper.Map<List<StudentFileModel>>(importedAccounts));
        }

        private async Task BoundStudentsToTheGroupAsync(List<StudentFileModel> importedAccounts, long groupId)
        {
            List<string> studentEmails = new List<string>();
            List<long> accountsIds = new List<long>();
            List<long> studentsIds = new List<long>();
            var newStudentStudentGroup = new List<StudentOfStudentGroup>();

            foreach (var account in importedAccounts)
            {
                studentEmails.Add(account.Email);
            }

            foreach (var account in await _accountService.GetAllNotAssignedAccountsAsync())
            {
                if (studentEmails.Contains(account.Email))
                {
                    accountsIds.Add(account.Id);
                }
            }

            foreach (var id in accountsIds)
            {
                await _studentService.CreateStudentAsync(id);
            }

            foreach (var student in await _studentService.GetAllActiveStudentsAsync())
            {
                if (studentEmails.Contains(student.Email))
                {
                    studentsIds.Add(student.Id);
                }
            }

            foreach (var studentId in studentsIds)
            {
                newStudentStudentGroup.Add(new StudentOfStudentGroup
                {
                    StudentGroupId = groupId,
                    StudentId = studentId
                });
            }

            _studentGroupService.AddStudentOfStudentGroups(newStudentStudentGroup);
            await _unitOfWork.CommitAsync();
        }


        private async Task IsValueValid(StudentFileModel fileLine, int rowCounter)
        {
            List<string> existingEmails = new List<string>();

            foreach (Account account in await _unitOfWork.AccountRepository.GetAllAsync())
            {
                existingEmails.Add(account.Email);
            }

            if (fileLine.FirstName == "")
            {
                throw new FormatException("Name field shouldn't be empty.\n" +
                    $"Problem was occured in col B, row {rowCounter}");
            }

            if (fileLine.LastName == "")
            {
                throw new FormatException("Name field shouldn't be empty.\n" +
                    $"Problem was occured in col C, row {rowCounter}");
            }

            if (existingEmails.Contains(fileLine.Email))
            {
                throw new DbUpdateException($"Account with email {fileLine.Email} already exists.\n" +
                   $"Problem was occured in col A, row {rowCounter}.");
            }
        }

        private bool IsEndOfFile(int rowCounter, IXLWorksheet studentsSheet)
        {
            return (studentsSheet.Cell($"A{rowCounter}").Value.ToString() == "")
               && (studentsSheet.Cell($"B{rowCounter}").Value.ToString() == "")
               && (studentsSheet.Cell($"C{rowCounter}").Value.ToString() == "");
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
