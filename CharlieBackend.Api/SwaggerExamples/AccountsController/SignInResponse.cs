using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
