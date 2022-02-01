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


        public SignInResponse GetExamples()
        {
            return new SignInResponse
            {
                first_name = "Evgeniy",
                last_name = "Karnaushekno",
                role = 1,
                role_list = new Dictionary<string, string>()
                {
                    {"student", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJTdHVkZW50IiwiSWQiOiI0IiwiRW1haWwiOiJ0aG9tYXMucm9iZXJ0c0BleGFtcGxlLmNvbSIsIkFjY291bnRJZCI6IjkiLCJuYmYiOjE2NDM3MTIyNDEsImV4cCI6MTY0Mzc1NTQ0MSwiaXNzIjoiTXlBdXRoU2VydmVyIiwiYXVkIjoiTXlBdXRoQ2xpZW50In0.1ITyYhlGsykxSJj8ftPLmYTV2Fik04dej6QX7HcEmyc" }
                } 
            };
        }
    }
}
