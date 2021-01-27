using CharlieBackend.Core.DTO.StudentGroups;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.ImportController
{
    internal class ImportDataFromFileResponse : IExamplesProvider<IEnumerable<ImportStudentGroupDto>>
    {
        public IEnumerable<ImportStudentGroupDto> GetExamples()
        {
            return new List<ImportStudentGroupDto>
            {
                new ImportStudentGroupDto
                {
                    Id = 42,
                    CourseId = 12,
                    Name = "Making of logo",
                    StartDate = new DateTime(2015, 7, 20),
                    FinishDate = new DateTime(2016, 8, 20)
                },
                new ImportStudentGroupDto
                {
                    Id = 43,
                    CourseId = 12,
                    Name = "Home design",
                    StartDate = new DateTime(2016, 8, 20),
                    FinishDate = new DateTime(2017, 9, 20)
                }
            };
        }
    }
}
