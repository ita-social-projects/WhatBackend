using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.ImportController
{
    class ImportGroupWithStudentsDataFromFileResponse : IExamplesProvider<GroupWithStudentsDto>
    {
        public GroupWithStudentsDto GetExamples()
        {
            return new GroupWithStudentsDto
            {
                CourseId = 1,
                Id = 11,
                Name = "AM-14",
                StartDate = DateTime.Parse("2015-07-20T00:00:00"),
                FinishDate = DateTime.Parse("2016-08-20T00:00:00"),
                Students = new List<StudentDto>
                {
                    new StudentDto
                    {
                        Id = 32,
                        Email = "student1@gmail.com",
                        FirstName = "Ivan",
                        LastName = "Ivanov"
                    },
                    new StudentDto
                    {
                        Id = 33,
                        Email = "student2@gmail.com",
                        FirstName = "Zahar",
                        LastName = "Zaharov"
                    },
                    new StudentDto
                    {
                        Id = 34,
                        Email = "student3@gmail.com",
                        FirstName = "John",
                        LastName = "Johnson"
                    }
                }
            };
        }
    }
}
