﻿using System;
using System.Linq;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Lesson;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Core.Models.Account;

namespace CharlieBackend.Core
{
    public static class AdapterExtensionMethods
    {
        public static Account ToAccount(this BaseAccountModel accountModel)
        {
            return new Account
            {
                Id = accountModel.Id,
                Email = accountModel.Email,
                Password = accountModel.Password,
                FirstName = accountModel.FirstName,
                LastName = accountModel.LastName,
                Role = (sbyte)accountModel.Role,
                IsActive = accountModel.IsActive
            };
        }

        public static BaseAccountModel ToAccountModel(this Account account)
        {
            // TODO: fix ID error
            return new BaseAccountModel
            {
                Id = account.Id,
                Email = account.Email,
                Password = account.Password,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Role = (sbyte)account.Role,
                IsActive = (bool)account.IsActive
            };
        }

        public static AccountInfoModel ToUserModel(this BaseAccountModel accountModel)
        {
            return new AccountInfoModel
            {
                FirstName = accountModel.FirstName,
                LastName = accountModel.LastName,
                Role = accountModel.Role
            };
        }

        public static Lesson ToLesson(this LessonModel lessonModel)
        {
            return new Lesson
            {
                Id = lessonModel.Id,
                LessonDate = lessonModel.LessonDate,
            };
        }

        public static LessonModel ToLessonModel(this Lesson lesson)
        {
            return new LessonModel
            {
                Id = lesson.Id,
                LessonDate = lesson.LessonDate,
                ThemeName = lesson.Theme?.Name,
            };
        }
    
 
        public static Mentor ToMentor(this MentorModel mentorModel)
        {
            return new Mentor
            {
                Id = mentorModel.Id
            };
        }

        public static MentorModel ToMentorModel(this Mentor mentor)
        {
            return new MentorModel
            {
                Id = mentor.Id,
                AccountId = mentor.AccountId,
                FirstName = mentor.Account.FirstName,
                LastName = mentor.Account.LastName,
                Email = mentor.Account.Email,
                Password = mentor.Account.Password,
                IsActive = (bool)mentor.Account.IsActive,
                CourseIds = mentor.MentorsOfCourses
                    .Select(mentorOfCourse => (long)mentorOfCourse.CourseId)
                    .ToList()
            };
        }
        public static StudentModel ToStudentModel(this Student student)
        {
            return new StudentModel
            {
                Id = student.Id,
                AccountId = student.AccountId,
                FirstName = student.Account.FirstName,
                LastName = student.Account.LastName,
                Email = student.Account.Email,
                Password = student.Account.Password,
                IsActive = (bool)student.Account.IsActive,
                StudentGroupIds = student.StudentsOfStudentGroups
                    .Select(studentOfStudentGroup => (long)studentOfStudentGroup
                    .StudentGroupId).ToList()
            };
        }
       
    }
}
