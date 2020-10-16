using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using Moq;
using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models.Course;
using CharlieBackend.Core.Models.Student;

namespace CharlieBackend.Api.Controllers.Tests
{
    
    public class StudentsControllerTests
    {
        [Fact]
        public async Task GetAllStudentsTestAsync()
        {
            var studentServiceMock = new Mock<IStudentService>();
            var accountServiceMock = new Mock<IAccountService>();
            studentServiceMock.Setup(repo => repo.GetAllStudentsAsync()).Returns(getStudents);
            StudentsController controller = new StudentsController
            (
                studentServiceMock.Object, 
                accountServiceMock.Object
            );
            var GetResult = controller.GetAllStudents();
            var taskResult = GetResult.Result.Result as ObjectResult;
            var toCompare = taskResult.Value as List<StudentModel>;
            var actualResult = await getStudents();
            Assert.Equal(toCompare.Count, actualResult.Count);
        }

        public async Task<List<StudentModel>> getStudents()
        {
            List<StudentModel> coursesL = new List<StudentModel>();
            coursesL.Add(new StudentModel { Id = 12, FirstName = "Testst1", LastName = "TestSt1" });
            coursesL.Add(new StudentModel { Id = 13, FirstName = "Testst2", LastName = "TestSt2" });
            coursesL.Add(new StudentModel { Id = 14, FirstName = "Testst3", LastName = "TestSt3" });
            return coursesL;
        }
    }

}