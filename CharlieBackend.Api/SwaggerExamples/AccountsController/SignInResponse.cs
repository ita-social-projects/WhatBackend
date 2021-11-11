using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.AccountsController
{
    internal class SignInResponse : IExamplesProvider<SignInResponse>
    {
        public string first_name { get; set; }

        public string last_name { get; set; }

        public int role { get; set; }

        public long id { get; set; }


        public SignInResponse GetExamples()
        {
            return new SignInResponse
            {
                first_name = "Evgeniy",
                last_name = "Karnaushekno",
                role = 1,
                id = 15
            };
        }


    }
}
