using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class StudentImportService 
    {
        #region private
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        public StudentImportService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public override async Task<Result<List<StudentFileModel>>> ImportFileAsync(ImportFileDto file)
        {
            var wb = GetFile(file.FileAsByte);
            var wsStudents = wb.Worksheet("Students");
            List<StudentFileModel> importedStudents = new List<StudentFileModel>();

            Type sgType = typeof(StudentFileModel);
            char charPointer = 'A';
            int rowCounter = 2;

            var properties = sgType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != Convert.ToString(wsStudents.Cell($"{charPointer}1").Value))
                {
                    return Result<List<StudentFileModel>>.GetError(ErrorCode.ValidationError,
                                "The format of the downloaded file is not suitable."
                                     + "Check headers in the file.");
                }
                charPointer++;
            }

            while (!IsEndOfFile(rowCounter, wsStudents))
            {

                try
                {
                    StudentFileModel fileLine = new StudentFileModel();

                    fileLine.Email = wsStudents.Cell($"A{rowCounter}").Value.ToString();
                    fileLine.FirstName = wsStudents.Cell($"B{rowCounter}").Value.ToString();
                    fileLine.LastName = wsStudents.Cell($"C{rowCounter}").Value.ToString();

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

                    importedStudents.Add(fileLine);
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

            await _unitOfWork.CommitAsync();

            return Result<List<StudentFileModel>>
                .GetSuccess(_mapper.Map<List<StudentFileModel>>(importedStudents));
        }

        protected override async Task IsValueValid(StudentFileModel fileLine, int rowCounter)
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

        protected override bool IsEndOfFile(int rowCounter, IXLWorksheet ws)
        {
            return (ws.Cell($"A{rowCounter}").Value.ToString() == "")
               && (ws.Cell($"B{rowCounter}").Value.ToString() == "")
               && (ws.Cell($"C{rowCounter}").Value.ToString() == "");
        }

        protected override XLWorkbook GetFile(byte[] fileAsBytes)
        {
            File.WriteAllBytes(@"C:\Users\Public\Student.xlsx", fileAsBytes);
            return new XLWorkbook(@"C:\Users\Public\Student.xlsx");
        }
    }
}
