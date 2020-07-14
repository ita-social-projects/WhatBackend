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
            var mock = new Mock<IStudentService>();
            var mock2 = new Mock<IAccountService>();
            mock.Setup(repo => repo.GetAllStudentsAsync()).Returns(students);
            StudentsController controller = new StudentsController(mock.Object, mock2.Object);
            var GetResult = controller.GetAllStudents();
            var a = GetResult.Result.Result as ObjectResult;
            var temp = a.Value as List<StudentModel>;
            var e = await students();
            Assert.Equal(e.Count, temp.Count);
        }
        public async Task<List<StudentModel>> students()
        {
            List<StudentModel> coursesL = new List<StudentModel>();
            coursesL.Add(new StudentModel { Id = 12, FirstName = "Testst1", LastName = "TestSt1" });
            coursesL.Add(new StudentModel { Id = 13, FirstName = "Testst2", LastName = "TestSt2" });
            coursesL.Add(new StudentModel { Id = 14, FirstName = "Testst3", LastName = "TestSt3" });
            return coursesL;
        }
    }

}