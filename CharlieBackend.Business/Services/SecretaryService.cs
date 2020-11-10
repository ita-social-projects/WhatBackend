using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

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

        public async Task<Result<SecretaryDto>> CreateSecretaryAsync(CreateSecretaryDto secretaryDto)
        {
            if (secretaryDto == null)
            {
                return Result<SecretaryDto>.Error(ErrorCode.UnprocessableEntity, "No secretary data received");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var isEmailTaken = await _accountService.IsEmailTakenAsync(secretaryDto.Email);

                    if (isEmailTaken)
                    {
                        return Result<SecretaryDto>.Error(ErrorCode.Conflict, "Email already taken");
                    };

                    var generatedPassword = _accountService.GenerateSalt();

                    var account = new Account
                    {
                        Email = secretaryDto.Email,
                        FirstName = secretaryDto.FirstName,
                        LastName = secretaryDto.LastName,
                        Role = Roles.Secretary
                    };

                    account.Salt = _accountService.GenerateSalt();
                    account.Password = _accountService.HashPassword(generatedPassword, account.Salt);

                    var secretary = new Secretary
                    {
                        Account = account,
                    };

                    _unitOfWork.SecretaryRepository.Add(secretary);

                    await _unitOfWork.CommitAsync();

                    if (await _credentialsSender.SendCredentialsAsync(account.Email, generatedPassword))
                    {
                        transaction.Commit();

                        return Result<SecretaryDto>.Success(_mapper.Map<SecretaryDto>(secretary));
                    }
                    else
                    {
                        //TODO implementation for resending email or sent a status msg
                        transaction.Commit();

                        return Result<SecretaryDto>.Success(_mapper.Map<SecretaryDto>(secretary));
                        //need to handle the exception with a right logic to sent it for contorller if fails
                        //throw new System.Exception("Faild to send credentials");
                    }
                }
                catch
                {
                    transaction.Rollback();

                    return Result<SecretaryDto>.Error(ErrorCode.InternalServerError, "Error while creating secretary");
                }
            }

        }

        public async Task<Result<SecretaryDto>> UpdateSecretaryAsync(UpdateSecretaryDto secretaryDto)
        {
            try
            {
                var foundSecretary = await _unitOfWork.SecretaryRepository
                        .GetByIdAsync(secretaryDto.Id);

                if (foundSecretary == null)
                {
                    return Result<SecretaryDto>.Error(ErrorCode.NotFound, "Secretary not found");
                }

                var isEmailChangableTo = await _accountService
                        .IsEmailChangableToAsync((long)foundSecretary.AccountId, secretaryDto.Email);

                if (!isEmailChangableTo)
                {
                    return Result<SecretaryDto>.Error(ErrorCode.Conflict, "Email is already taken");
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

                return Result<SecretaryDto>.Success(_mapper.Map<SecretaryDto>(foundSecretary));

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<Result<SecretaryDto>> GetSecretaryByAccountIdAsync(long accountId)
        {
            var secretary = await _unitOfWork
                .SecretaryRepository.GetSecretaryByAccountIdAsync(accountId);

            return Result<SecretaryDto>.Success(_mapper.Map<SecretaryDto>(secretary));
        }

        public async Task<Result<SecretaryDto>> GetSecretaryByIdAsync(long secretaryId)
        {
            var secretary = await _unitOfWork.SecretaryRepository.GetByIdAsync(secretaryId);

            return Result<SecretaryDto>.Success(_mapper.Map<SecretaryDto>(secretary));
        }

        public async Task<long?> GetAccountId(long secretaryId)
        {
            var secretary = await _unitOfWork.SecretaryRepository.GetByIdAsync(secretaryId);

            return secretary?.Id;
        }

        public async Task<Result<IList<SecretaryDto>>> GetAllSecretariesAsync()
        {
            var secretaries = await _unitOfWork.SecretaryRepository.GetAllAsync();

            return Result<IList<SecretaryDto>>.Success(_mapper.Map<IList<SecretaryDto>>(secretaries));
        }

        public async Task<Result<SecretaryDto>> DisableSecretaryAsync(long secretaryId)
        {
            var accountId = await GetAccountId(secretaryId);

            if (accountId == null)
            {
                return Result<SecretaryDto>.Error(ErrorCode.NotFound, "Unknown secretary id.");
            }

            var isDisabled = await _accountService.DisableAccountAsync((long)accountId);

            if (isDisabled)
            {
                return Result<SecretaryDto>.Success(null);
            }

            return Result<SecretaryDto>.Error(ErrorCode.InternalServerError,
                "Error occurred while trying to disable secretary account.");
        }
    }
}
