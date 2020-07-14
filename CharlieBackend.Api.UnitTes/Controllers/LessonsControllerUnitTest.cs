using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using Moq;
using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Api.Controllers;
using CharlieBackend.Core.Models.Lesson;

namespace CharlieBackend.Api.UnitTest.Controllers
{
    public class LessonsControllerUnitTest
    {
        [Fact]
        public async Task GetAllLessons()
        {
            var mock = new Mock<ILessonService>();
            mock.Setup(repo => repo.GetAllLessonsAsync()).Returns(lessons);
            LessonsController controller = new LessonsController(mock.Object);
            var GetResult = controller.GetAllLessons();
            var a = GetResult.Result.Result as ObjectResult;
            var temp = a.Value as List<LessonModel>;
            var e = await lessons();
            Assert.Equal(e.Count, temp.Count);
        }
        public async Task<List<LessonModel>> lessons() { 
            List<LessonModel> less = new List<LessonModel>();
            less.Add(new LessonModel { Id = 1, ThemeName = "Testst1", LessonDate = "TestSt1" });
            less.Add(new LessonModel { Id = 13, ThemeName = "Testst2", LessonDate = "TestSt2" });
            less.Add(new LessonModel { Id = 14, ThemeName = "Testst3", LessonDate = "TestSt3" });
            return less;
        }
    }
}
