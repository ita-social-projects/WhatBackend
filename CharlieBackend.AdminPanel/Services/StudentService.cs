using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class StudentService: IStudentService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IOptions<ApplicationSettings> _config;

        public StudentService(IApiUtil apiUtil, IOptions<ApplicationSettings> config)
        {
            _apiUtil = apiUtil;
            _config = config;
        }

        public async Task<IList<StudentViewModel>> GetAllStudentsAsync(string accessToken)
        {
            var students = await _apiUtil.GetAsync<IList<StudentViewModel>>($"{_config.Value.Urls.Api.Https}/api/students", accessToken);

            return students;
        }
    }
}
