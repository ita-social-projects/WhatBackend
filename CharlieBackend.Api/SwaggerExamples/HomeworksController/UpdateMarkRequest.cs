using CharlieBackend.Core.DTO.Homework;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.HomeworksController
{
    public class UpdateMarkRequest : IExamplesProvider<UpdateMarkRequestDto>
    {
        public UpdateMarkRequestDto GetExamples()
        {
            return new UpdateMarkRequestDto
            {
                StudentHomeworkId = 1,
                StudentMark = 5
            };
        }
    }
}
