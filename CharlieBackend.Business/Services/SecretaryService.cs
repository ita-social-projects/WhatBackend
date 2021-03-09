using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Linq;

namespace CharlieBackend.Business.Services
{
    public class SecretaryService : ISecretaryService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;
        private readonly IBlobService _blobService;

        public SecretaryService(IAccountService accountService, IUnitOfWork unitOfWork,
                                IMapper mapper, INotificationService notification, IBlobService blobService)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notification = notification;
            blobService = _blobService;
        }

        public async Task<Result<SecretaryDto>> CreateSecretaryAsync(long accountId)
        {
            try
            {
                var account = await _accountService.GetAccountCredentialsByIdAsync(accountId);

                if (account == null)
                {
                    return Result<SecretaryDto>.GetError(ErrorCode.NotFound,
                        "Account not found");
                }

                if (account.Role == UserRole.NotAssigned)
                {
                    account.Role = UserRole.Secretary;

                    var secretary = new Secretary
                    {
                        Account = account,
                        AccountId = accountId
                    };

                    _unitOfWork.SecretaryRepository.Add(secretary);

                    await _unitOfWork.CommitAsync();

                    await _notification.AccountApproved(account);

                    return Result<SecretaryDto>.GetSuccess(_mapper.Map<SecretaryDto>(secretary));
                } 
                else
                {
                    _unitOfWork.Rollback();

                    return Result<SecretaryDto>.GetError(ErrorCode.ValidationError,
                       "This account already assigned.");
                }
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<SecretaryDto>.GetError(ErrorCode.InternalServerError, "Error while creating secretary");
            }

        }

        public async Task<Result<SecretaryDto>> UpdateSecretaryAsync(long secretaryId, UpdateSecretaryDto secretaryDto)
        {
            try
            {
                var foundSecretary = await _unitOfWork.SecretaryRepository
                        .GetByIdAsync(secretaryId);

                if (foundSecretary == null)
                {
                    return Result<SecretaryDto>.GetError(ErrorCode.NotFound, "Secretary not found");
                }

                var isEmailChangableTo = await _accountService
                        .IsEmailChangableToAsync((long)foundSecretary.AccountId, secretaryDto.Email);

                if (!isEmailChangableTo)
                {
                    return Result<SecretaryDto>.GetError(ErrorCode.Conflict, "Email is already taken");
                }

                foundSecretary.Account.Email = secretaryDto.Email ?? foundSecretary.Account.Email;
                foundSecretary.Account.FirstName = secretaryDto.FirstName ?? foundSecretary.Account.FirstName;
                foundSecretary.Account.LastName = secretaryDto.LastName ?? foundSecretary.Account.LastName;

                await _unitOfWork.CommitAsync();

                return Result<SecretaryDto>.GetSuccess(_mapper.Map<SecretaryDto>(foundSecretary));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<SecretaryDto>.GetError(ErrorCode.InternalServerError,
                      "Cannot update mentor.");
            }
        }

        public async Task<Result<SecretaryDto>> GetSecretaryByAccountIdAsync(long accountId)
        {
            var secretary = await _unitOfWork
                .SecretaryRepository.GetSecretaryByAccountIdAsync(accountId);

            return Result<SecretaryDto>.GetSuccess(_mapper.Map<SecretaryDto>(secretary));
        }

        public async Task<Result<SecretaryDto>> GetSecretaryByIdAsync(long secretaryId)
        {
            var secretary = await _unitOfWork.SecretaryRepository.GetByIdAsync(secretaryId);

            return Result<SecretaryDto>.GetSuccess(_mapper.Map<SecretaryDto>(secretary));
        }

        public async Task<long?> GetAccountId(long secretaryId)
        {
            var secretary = await _unitOfWork.SecretaryRepository.GetByIdAsync(secretaryId);

            return secretary?.AccountId;
        }

        public async Task<IList<SecretaryDetailsDto>> GetAllSecretariesAsync()
        {
            var secretaries = await GetSecretariesWithAvatarIncluded(await _unitOfWork.SecretaryRepository.GetAllAsync());

            return secretaries;
        }

        private async Task<IList<SecretaryDetailsDto>> GetSecretariesWithAvatarIncluded(IList<Secretary> secretaries)
        {
            var detailsDtos = await secretaries
                .ToAsyncEnumerable()
                .Select(m =>
                {
                    var detailsDto = _mapper.Map<SecretaryDetailsDto>(m);

                    detailsDto.AvatarUrl = m.Account.Avatar != null ? _blobService.GetUrl(m.Account.Avatar) : null;

                    return detailsDto;
                })
                .Select(x => x)
                .ToListAsync();

            return detailsDtos;
        }

        public async Task<Result<IList<SecretaryDetailsDto>>> GetActiveSecretariesAsync()
        {
            var secretaries = await GetSecretariesWithAvatarIncluded(await _unitOfWork.SecretaryRepository.GetActiveAsync());

            return Result<IList<SecretaryDetailsDto>>.GetSuccess(secretaries);
        }

        public async Task<Result<bool>> DisableSecretaryAsync(long secretaryId)
        {
            var accountId = await GetAccountId(secretaryId);

            if (accountId == null)
            {
                return Result<bool>.GetError(ErrorCode.NotFound, "Unknown secretary id.");
            }

            var changedToDisabled = await _accountService.DisableAccountAsync((long)accountId);

            if (!changedToDisabled)
            {
                return Result<bool>.GetError(ErrorCode.Conflict,"This secretsry account is already disabled.");
            }

            return Result<bool>.GetSuccess(changedToDisabled);
        }

        public async Task<Result<bool>> EnableSecretaryAsync(long secretaryId)
        {
            var accountId = await GetAccountId(secretaryId);

            if (accountId == null)
            {
                return Result<bool>.GetError(ErrorCode.NotFound, "Unknown secretary id.");
            }

            var changedToEnabled = await _accountService.EnableAccountAsync((long)accountId);

            if (!changedToEnabled)
            {
                return Result<bool>.GetError(ErrorCode.Conflict, "This secretsry account is already enabled.");
            }

            return Result<bool>.GetSuccess(changedToEnabled);
        }
    }
}
