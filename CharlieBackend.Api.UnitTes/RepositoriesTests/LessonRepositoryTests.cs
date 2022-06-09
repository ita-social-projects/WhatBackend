using CharlieBackend.Api.UnitTest.AutoData.CustomAutoDataAttributes;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data;
using CharlieBackend.Data.Exceptions;
using CharlieBackend.Data.Repositories.Impl;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.RepositoriesTests
{
    public class LessonRepositoryTests
    {
        [Theory, LessonsRepositoryAutoData]
        public async Task UpdateMarkAsyncTest_ValidDataPassed_ShouldReturnVisit(
            Lesson lesson, Homework homework, 
            Visit visit1, Visit visit2, 
            HomeworkStudent homeworkStudent1, HomeworkStudent homeworkStudent2
            )
        {
            visit1.Presence = true;
            visit2.Presence = false;

            lesson.Visits.Add(visit1);
            lesson.Visits.Add(visit2);

            homeworkStudent1.HomeworkId = homework.Id;
            homeworkStudent2.HomeworkId = homework.Id;

            homeworkStudent1.StudentId = visit1.StudentId;
            homeworkStudent2.StudentId = visit2.StudentId;

            homework.LessonId = lesson.Id;
            homework.Lesson = lesson;

            homework.HomeworkStudents.Add(homeworkStudent1);
            homework.HomeworkStudents.Add(homeworkStudent2);

            homeworkStudent1.Homework = homework;
            visit1.Lesson = lesson;

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            using (var context = new ApplicationContext(options))
            {
                context.Database.EnsureCreated();
                context.Lessons.Add(lesson);
                context.Homeworks.Add(homework);
                context.HomeworkStudents.Add(homeworkStudent1);
                context.HomeworkStudents.Add(homeworkStudent2);
                context.Visits.Add(visit1);
                context.Visits.Add(visit2);
                context.SaveChanges();
            }

            using (var context = new ApplicationContext(options))
            {
                var lessonRepository = new LessonRepository(context);

                // Act
                var foundVisitTrue = await lessonRepository.GetVisitByStudentHomeworkIdAsync(homeworkStudent1.Id);
                var foundVisitFalse = await lessonRepository.GetVisitByStudentHomeworkIdAsync(homeworkStudent2.Id);

                // Assert
                foundVisitTrue.Id.Should().Equals(visit1.Id);
                foundVisitFalse.Id.Should().Equals(visit2.Id);
            }
        }

        [Fact]
        public async Task UpdateMarkAsyncTest_NoDataExisted_ShouldThrowException()
        {
            // Arrange
            int wrongHomeworkStudentId = 1;
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "EmptyTestDatabase").Options;

            using (var context = new ApplicationContext(options))
            {
                var lessonRepository = new LessonRepository(context);

                // Act
                Func<Task> wrongRequest = async () => await lessonRepository.GetVisitByStudentHomeworkIdAsync(wrongHomeworkStudentId);

                // Assert
                await wrongRequest.Should().ThrowAsync<NotFoundException>();
            }
        }

    }
}
