using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class GetStudentByIdResponse : IExamplesProvider<StudentMock>
    {
        public StudentMock GetExamples()
        {
            return new StudentMock
            {
                First_name = "Brad",
                Last_name = "Pitt",
                Email = "student@example.com"
            };
        }
    }

    class StudentMock
    {
        public string First_name {get; set;}

        public string Last_name { get; set; }

        public string Email { get; set; }
    }
}
