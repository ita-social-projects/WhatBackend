using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.AccountsController
{
    internal class SignInResponse : IExamplesProvider<SignInResponse>
    {
        public string first_name { get; set; }

        public string last_name { get; set; }

        public int role { get; set; }

        public Dictionary<string, string> role_list { get; set; }

        public string localization { get; set; }


        public SignInResponse GetExamples()
        {
            return new SignInResponse
            {
                first_name = "Evgeniy",
                last_name = "Karnaushekno",
                role = 1,
                role_list = new Dictionary<string, string>()
                {
                    {"student", "Bearer yJhbGciOiJIUzI1iIsInR5cCI6IkpXVCJ9" }
                } 
            };
        }
    }
}
