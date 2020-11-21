using CharlieBackend.Core.FileModels;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.SwaggerExamples.ImportController
{
    internal class ImportDataFromFileResponse : IExamplesProvider<List<StudentGroupFileModel>>
    {
        public List<StudentGroupFileModel> GetExamples()
        {
            return new List<StudentGroupFileModel>
            {
                new StudentGroupFileModel
                {
                    Id = "42",
                    CourseId = "12",
                    Name = "Making of logo",
                    StartDate = new DateTime(2015, 7, 20),
                    FinishDate = new DateTime(2016, 8, 20)
                },
                new StudentGroupFileModel
                {
                    Id = "45",
                    CourseId = "14",
                    Name = "Home design",
                    StartDate = new DateTime(2016, 8, 20),
                    FinishDate = new DateTime(2017, 9, 20)
                }
            };
        }
    }
}
