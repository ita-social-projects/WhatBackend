using CharlieBackend.Core.DTO.StudentGroups;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.StudentGroupsController
{
    class GetAllStudentGroupsResponse : IExamplesProvider<IList<StudentGroupDto>>
    {
        public IList<StudentGroupDto> GetExamples()
        {
            return new List<StudentGroupDto>()
            {
                new StudentGroupDto()
                {
                    Id = 212,
                    Name = "SX-13",
                    CourseId = 17,
                    StartDate = new DateTime(2015, 7, 20),
                    FinishDate = new DateTime(2015, 9, 20),
                    MentorIds = new List<long> { 12, 43 },
                    StudentIds = new List<long> { 33, 42, 43 }
                },
                new StudentGroupDto()
                {
                    Id = 214,
                    Name = "SX-15",
                    CourseId = 18,
                    StartDate = new DateTime(2016, 7, 20),
                    FinishDate = new DateTime(2016, 9, 20),
                    MentorIds = new List<long> { 13, 47 },
                    StudentIds = new List<long> { 11, 46, 38 }
                }

            };
        }
    }
}
