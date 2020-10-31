using System;
using System.Linq;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Visit;
using CharlieBackend.Core.Models.Lesson;
using CharlieBackend.Core.Models.Student;


namespace CharlieBackend.Core
{
    public static class AdapterExtensionMethods
    {
        

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

        public static VisitModel ToVisitModel(this Visit visit)
        {
            return new VisitModel
            {
                StudentId = (long)visit.StudentId,
                Comment = visit.Comment,
                StudentMark = visit.StudentMark,
                Presence = visit.Presence

            };
        }

 
        public static Mentor ToMentor(this MentorModel mentorModel)
        {
            return new Mentor
            {
                //Id = mentorModel.Id
            };
        }

        public static MentorModel ToMentorModel(this Mentor mentor)
        {
            return new MentorModel
            {
               // Id = mentor.Id,
               // FirstName = mentor.Account.FirstName,
               // LastName = mentor.Account.LastName,
               // Email = mentor.Account.Email,
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
               // Id = student.Id,
               // FirstName = student.Account.FirstName,
               // LastName = student.Account.LastName,
                Email = student.Account.Email,
                Password = student.Account.Password,
                IsActive = (bool)student.Account.IsActive,
                StudentGroupIds = student.StudentsOfStudentGroups
                    .Select(studentOfStudentGroup => (long)studentOfStudentGroup
                    .StudentGroupId).ToList()
            };
        }
        public static Student ToStudent(this StudentModel studentModel)
        {
            return new Student
            {     
            };
        }

        
    }
}
