using CharlieBackend.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using Moq;
using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models.StudentGroup;

namespace CharlieBackend.Api.Controllers.Tests
{
   
    public class StudentGroupsControllerTests
    {
        
        [Fact]
        public async Task GetAllStudentGroupsTestAsync()
        {
            var mock = new Mock<IStudentGroupService>();
            mock.Setup(repo => repo.GetAllStudentGroupsAsync()).Returns(getStudentGroups);
            StudentGroupsController controller = new StudentGroupsController(mock.Object);
            var GetResult = controller.GetAllStudentGroups();
            var taskResult = GetResult.Result.Result as ObjectResult;
            var toCompare = taskResult.Value as List<StudentGroupModel>;
            var actualResult = await getStudentGroups();
            Assert.Equal(toCompare.Count, actualResult.Count);
        }
        public async Task<List<StudentGroupModel>> getStudentGroups()
        {
            List<StudentGroupModel> studentG = new List<StudentGroupModel>()
            {
                new StudentGroupModel { Id = 12, Name="Testst1", StartDate="26.06.2020", FinishDate="20.10.2020" },
                new StudentGroupModel { Id = 13, Name = "Testst2", StartDate = "20.05.2020", FinishDate = "13.08.2020" },
                new StudentGroupModel { Id = 14, Name = "Testst3", StartDate = "14.04.2020", FinishDate = "18.09.2020" }
            };
            return studentG;
        }
    }
}