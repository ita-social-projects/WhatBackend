using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;

namespace CharlieBackend.Business.Services
{
    public class SecretaryService : ISecretaryService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICredentialsSenderService _credentialsSender;

        public SecretaryService(IAccountService accountService, IUnitOfWork unitOfWork, IMapper mapper, ICredentialsSenderService credentialsSender)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _credentialsSender = credentialsSender;
        }

        public async Task<SecretaryDto> CreateSecretaryAsync(CreateSecretaryDto secretaryDto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    bool credsSent = false;
                    var generatedPassword = _accountService.GenerateSalt();

                    var account = new Account
                    {
                        Email = secretaryDto.Email,
                        FirstName = secretaryDto.FirstName,
                        LastName = secretaryDto.LastName,
                        Role = 3
                    };

                    account.Salt = _accountService.GenerateSalt();
                    account.Password = _accountService.HashPassword(generatedPassword, account.Salt);

                    var secretary = new Secretary
                    {
                        Account = account,
                        Id = secretaryDto.Id
                    };

                    _unitOfWork.SecretaryRepository.Add(secretary);


                    await _unitOfWork.CommitAsync();

                    if (await _credentialsSender.SendCredentialsAsync(account.Email, generatedPassword))
                    {
                        credsSent = true;
                        transaction.Commit();

                        return _mapper.Map<SecretaryDto>(secretary);
                    }
                    else
                    {
                        //TODO implementation for resending email or sent a status msg
                        transaction.Commit();
                        credsSent = false;
                        return _mapper.Map<SecretaryDto>(secretary);
                        //need to handle the exception with a right logic to sent it for contorller if fails
                        //throw new System.Exception("Faild to send credentials");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Debug info exception details: " + ex);
                    return null;
                }
            }

        }

        public async Task<SecretaryDto> UpdateSecretaryAsync(UpdateSecretaryDto secretaryDto)
        {
            try
            {
                var foundSecretary = await _unitOfWork.MentorRepository.GetByIdAsync(secretaryDto.Id);

                if (foundSecretary == null)
                {
                    return null;
                }

                foundSecretary.Account.Email = secretaryDto.Email ?? foundSecretary.Account.Email;
                foundSecretary.Account.FirstName = secretaryDto.FirstName ?? foundSecretary.Account.FirstName;
                foundSecretary.Account.LastName = secretaryDto.LastName ?? foundSecretary.Account.LastName;

                if (!string.IsNullOrEmpty(secretaryDto.Password))
                {
                    foundSecretary.Account.Salt = _accountService.GenerateSalt();
                    foundSecretary.Account.Password = _accountService.HashPassword(secretaryDto.Password, foundSecretary.Account.Salt);
                }

                await _unitOfWork.CommitAsync();
                return _mapper.Map<SecretaryDto>(foundSecretary);

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<SecretaryDto> GetSecretaryByAccountIdAsync(long accountId)
        {
            var secretary = await _unitOfWork.SecretaryRepository.GetSecretaryByAccountIdAsync(accountId);

            return _mapper.Map<SecretaryDto>(secretary);
        }

        public async Task<SecretaryDto> GetSecretaryByIdAsync(long secretaryId)
        {
            var secretary = await _unitOfWork.SecretaryRepository.GetByIdAsync(secretaryId);

            return _mapper.Map<SecretaryDto>(secretary);
        }

        public async Task<long?> GetAccountId(long secretaryId)
        {
            var secretary = await _unitOfWork.SecretaryRepository.GetByIdAsync(secretaryId);

            return secretary?.Id;
        }

        public async Task<IList<SecretaryDto>> GetAllSecretariesAsync()
        {
            var secretaries = await _unitOfWork.SecretaryRepository.GetAllAsync();

            return _mapper.Map<IList<SecretaryDto>>(secretaries);
        }
    }
}
