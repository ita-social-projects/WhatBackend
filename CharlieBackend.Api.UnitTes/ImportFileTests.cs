using CharlieBackend.Business.Services.FileServices;
using CharlieBackend.Business.Services.FileServices.Importers;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class ImportFileTests
    {
        private Mock<IAccountService> _accountServiceMock;
        private Mock<IStudentGroupService> _studentGroupServiceMock;
        private Mock<IStudentService> _studentServiceMock;

        private const int STUDENTS_COUNT = 3;

        IList<CreateAccountDto> _readAccounts = new List<CreateAccountDto>
        {
                new CreateAccountDto
                {
                    FirstName = "FNAme_First",
                    LastName = "LName_First",
                    Email = "Email_First"
                },
                new CreateAccountDto
                {
                    FirstName = "FNAme_Second",
                    LastName = "LName_Second",
                    Email = "Email_Second"
                },
                new CreateAccountDto
                {
                    FirstName = "FNAme_Third",
                    LastName = "LName_Third",
                    Email = "Email_Third"
                }
        };

        IList<StudentDto> _createdStudents = new List<StudentDto>
        {
            new StudentDto
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName"
            },
            new StudentDto
            {
                Id = 2,
                FirstName = "FirstName",
                LastName = "LastName"
            },
            new StudentDto
            {
                Id = 3,
                FirstName = "FirstName",
                LastName = "LastName"
            },
        };

        private AccountDto _testAccounts;

        public ImportFileTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _studentGroupServiceMock = new Mock<IStudentGroupService>();
            _studentServiceMock = new Mock<IStudentService>();

            _testAccounts = new AccountDto { Email = "test_email" };
        }

        [Fact]
        public async Task UploadFileAsync_CreateFile_FileExist()
        {
            //Arrange
            string fileName = "file_Test.csv";
            var fileMock = new Mock<IFormFile>();
            var fileServiceMock = new Mock<FileService>();

            fileMock.Setup(_ => _.FileName).Returns(fileName);

            //Action
            var path = await fileServiceMock.Object.UploadFileAsync(fileMock.Object);

            //Assert
            Assert.True(File.Exists(path));
            fileServiceMock.Object.Dispose();
        }

        [Fact]
        public async Task ImportCsvGroupAsync_ImportExistingStudent_CreateGroup()
        {
            //Arrange
            #region Check and get accounts from db

            _accountServiceMock
                .Setup(s => s.IsEmailTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _accountServiceMock
                .Setup(s => s.GetAccountCredentialsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<AccountDto>.GetSuccess(_testAccounts));

            #endregion

            #region Check and get students from db

            var result = Result<StudentDto>.GetSuccess(new StudentDto());
            var expectedStudentIds = _createdStudents.Select(s => s.Id).ToList();
            int studentCount = 0;

            _studentServiceMock
                .Setup(s => s.GetStudentByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(result)
                .Callback(() => result.Data = _createdStudents[studentCount++]);

            #endregion

            _studentGroupServiceMock
                .Setup(s => s.CreateStudentGroupAsync(It.IsAny<CreateStudentGroupDto>()))
                .ReturnsAsync(Result<StudentGroupDto>.GetSuccess(new StudentGroupDto()));

            var groupCsvFileImporter = new StudentGroupImporter(
                _studentServiceMock.Object,
                _studentGroupServiceMock.Object,
                _accountServiceMock.Object);

            //Act
            var resultFinish = await groupCsvFileImporter.ImportGroupAsync(
                new CreateStudentGroupDto(), _readAccounts);

            var resultStudentIds = resultFinish.Data.Students.Select(s => s.Id).ToList();

            //Assert
            _accountServiceMock.Verify(s => s.IsEmailTakenAsync(
                It.IsAny<string>()), Times.Exactly(STUDENTS_COUNT));

            _accountServiceMock.Verify(s => s.GetAccountCredentialsByEmailAsync(
                It.IsAny<string>()), Times.Exactly(STUDENTS_COUNT));

            _studentServiceMock.Verify(s => s.GetStudentByEmailAsync(
                It.Is<string>(val => val.Equals(_testAccounts.Email))),
                Times.Exactly(STUDENTS_COUNT));

            _studentGroupServiceMock.Verify(s => s.CreateStudentGroupAsync(
                It.IsAny<CreateStudentGroupDto>()));

            Assert.Equal(expectedStudentIds, resultStudentIds);
        }

        [Fact]
        public async Task ImportCsvGroupAsync_ImportExistingAccountNotStudent_CreateGroup()
        {
            //Arrange
            #region Check and get account from db

            _accountServiceMock
                .Setup(s => s.IsEmailTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _accountServiceMock
                .Setup(s => s.GetAccountCredentialsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<AccountDto>.GetSuccess(_testAccounts));

            #endregion

            #region Check and create students

            _studentServiceMock
                .Setup(s => s.GetStudentByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<StudentDto>.GetError(ErrorCode.NotFound,
                    "Student wasn't found by email"));

            var result = Result<StudentDto>.GetSuccess(new StudentDto());
            var expectedStudentIds = _createdStudents.Select(s => s.Id).ToList();
            int studentCount = 0;

            _studentServiceMock
                .Setup(s => s.CreateStudentAsync(It.IsAny<long>()))
                .ReturnsAsync(result)
                .Callback(() => result.Data = _createdStudents[studentCount++]);

            #endregion

            _studentGroupServiceMock
                .Setup(s => s.CreateStudentGroupAsync(It.IsAny<CreateStudentGroupDto>()))
                .ReturnsAsync(Result<StudentGroupDto>.GetSuccess(new StudentGroupDto()));

            var groupCsvFileImporter = new StudentGroupImporter(
                _studentServiceMock.Object,
                _studentGroupServiceMock.Object,
                _accountServiceMock.Object);

            //Act
            var resultFinish = await groupCsvFileImporter.ImportGroupAsync(
                new CreateStudentGroupDto(), _readAccounts);

            var resultStudentIds = resultFinish.Data.Students.Select(s => s.Id).ToList();

            //Assert
            _accountServiceMock.Verify(s => s.IsEmailTakenAsync(
                It.IsAny<string>()), Times.Exactly(STUDENTS_COUNT));

            _accountServiceMock.Verify(s => s.GetAccountCredentialsByEmailAsync(
                It.IsAny<string>()), Times.Exactly(STUDENTS_COUNT));

            _studentServiceMock.Verify(s => s.GetStudentByEmailAsync(
                It.Is<string>(val => val.Equals(_testAccounts.Email))),
                Times.Exactly(STUDENTS_COUNT));

            _studentServiceMock.Verify(s => s.CreateStudentAsync(
                It.IsAny<long>()), Times.Exactly(STUDENTS_COUNT));

            _studentGroupServiceMock.Verify(s => s.CreateStudentGroupAsync(
                It.IsAny<CreateStudentGroupDto>()));

            Assert.Equal(expectedStudentIds, resultStudentIds);
        }
    }
}
