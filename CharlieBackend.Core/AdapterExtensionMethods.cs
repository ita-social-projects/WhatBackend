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
                Id = accountModel.Id,
                Email = accountModel.Email,
                Password = accountModel.Password,
                FirstName = accountModel.FirstName,
                LastName = accountModel.LastName,
                Role = (sbyte)accountModel.Role
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
                Role = (sbyte)account.Role
            };
        }

        public static Lesson ToLesson(this LessonModel lessonModel)
        {
            return new Lesson
            {
                Id = lessonModel.Id,
                StudentGroupId = lessonModel.group_id,
                LessonDate = DateTime.Parse(lessonModel.lesson_date),
            };
        }

        public static LessonModel ToLessonModel(this Lesson lesson)
        {
            return new LessonModel
            {
                Id = lesson.Id,
                group_id = lesson.StudentGroupId ?? 0, // TODO: remove
                lesson_date = lesson.LessonDate.ToString()
            };
        }

        public static Course ToCourse(this CourseModel courseModel)
        {
            return new Course
            {
                Id = courseModel.Id,
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

        //public static StudentGroup ToStudentGroup(this StudentGroupModel studentGroupModel)
        //{
        //    return new StudentGroup
        //    {
        //        Name = studentGroupModel.name,
        //        CourseId = studentGroupModel.course_id,
        //        StartDate = DateTime.Parse(studentGroupModel.start_date),
        //        FinishDate = DateTime.Parse(studentGroupModel.finish_date),
        //    };
        //}

        public static Theme ToTheme(this ThemeModel themeModel)
        {
            return new Theme
            {
                Id = themeModel.Id,
                Name = themeModel.Name
            };
        }

        public static ThemeModel ToThemeModel(this Theme themeModel)
        {
            return new ThemeModel
            {
                Id = themeModel.Id,
                Name = themeModel.Name
            };
        }

        public static Mentor ToMentor(this MentorModel mentorModel)
        {
            return new Mentor
            {
               
            };
        }
    }
}
