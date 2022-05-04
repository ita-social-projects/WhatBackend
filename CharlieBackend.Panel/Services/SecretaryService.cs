using CharlieBackend.Panel.Models.Secretary;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Secretary;

namespace CharlieBackend.Panel.Services
{
    public class SecretaryService : ISecretaryService
    {
        private readonly IApiUtil _apiUtil;
        private readonly SecretariesApiEndpoints _secretariesApiEndpoints;        

        public SecretaryService(IApiUtil apiUtil, IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;
            _secretariesApiEndpoints = options.Value.Urls.ApiEndpoints.Secretaries;            
        }

        public async Task<IList<SecretaryViewModel>> GetAllSecretariesAsync()
        {
            var getAllSecretariesEndpoint = _secretariesApiEndpoints.GetAllSecretariesEndpoint;

            var allSecretaries = await
                _apiUtil.GetAsync<IList<SecretaryViewModel>>(getAllSecretariesEndpoint);

            return allSecretaries;
        }

        public async Task<bool> EnableSecretaryAsync(long id)
        {
            var enableSecretaryEndpoint = string
                .Format(_secretariesApiEndpoints.EnableSecretaryEndpoint, id);

            return await _apiUtil.EnableAsync<bool>(enableSecretaryEndpoint);
        }

        public async Task<bool> DisableSecretaryAsync(long id)
        {
            var disableSecretaryEndpoint = string
                .Format(_secretariesApiEndpoints.DisableSecretaryEndpoint, id);

            return await _apiUtil.DeleteAsync<bool>(disableSecretaryEndpoint);
        }

        public async Task<SecretaryEditViewModel> GetSecretaryByIdAsync(long id)
        {
            var getSecretaryEndpoint = string
                .Format(_secretariesApiEndpoints.GetSecretaryEndpoint, id);

            var secretary = await _apiUtil.GetAsync<SecretaryEditViewModel>(getSecretaryEndpoint);

            return secretary;
        }

        public async Task<UpdateSecretaryDto> UpdateSecretaryAsync(long id, UpdateSecretaryDto UpdateDto)
        {
            var updateSecretaryEndpoint = string
                .Format(_secretariesApiEndpoints.UpdateSecretaryEndpoint, id);

            var updatedSecretary = await _apiUtil.PutAsync(updateSecretaryEndpoint, UpdateDto);

            return updatedSecretary;
        }
    }
}