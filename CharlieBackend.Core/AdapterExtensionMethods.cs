using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using System;

namespace CharlieBackend.Core
{
    public static class AdapterExtensionMethods
    {
        public static Account ToAccount(this AccountModel accountModel)
        {
            return new Account
            {
                Email = accountModel.Email,
                Password = accountModel.Password,
                FirstName = accountModel.FirstName,
                LastName = accountModel.LastName,
                Role = accountModel.Role
            };
        }

        public static AccountModel ToAccountModel(this Account account)
        {
            return new AccountModel
            {
                Id = account.Id,
                Email = account.Email,
                Password = account.Password,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Role = account.Role
            };
        }

        public static Lesson ToLesson(this LessonModel lessonModel)
        {
            return new Lesson
            {
                IdStudentGroup = lessonModel.group_id,
                LessonDate = DateTime.Parse(lessonModel.lesson_date),
            };
        }

        public static LessonModel ToLessonModel(this Lesson lesson)
        {
            return new LessonModel
            {
                Id = lesson.Id,
                group_id = lesson.IdStudentGroup ?? 0, // TODO: remove
                lesson_date = lesson.LessonDate.ToString()
            };
        }

        public static Course ToCourse(this CourseModel courseModel)
        {
            return new Course
            {
                Name = courseModel.Name
            };
        }

        public static CourseModel ToCourseModel(this Course course)
        {
            return new CourseModel
            {
                Id = course.Id,
                Name = course.Name,
            };
        }
    }
}
