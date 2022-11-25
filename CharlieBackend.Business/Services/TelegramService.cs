using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeSpan _telegramTokenValidTime;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public TelegramService(IUnitOfWork unitOfWork,
                              ICurrentUserService currentUserService,
                              IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _telegramTokenValidTime = new TimeSpan(1, 0, 0);
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<string>> GetTelegramBotLink()
        {
            var user = await _unitOfWork.AccountRepository
                    .GetByIdAsync(_currentUserService.AccountId);

            Result<string> result;

            if (user == null)
            {
                result = Result<string>.GetError(ErrorCode.NotFound,
                    "Account not found");
            }
            else if (user.TelegramId == null || user.TelegramId == string.Empty)
            {
                //todo: move link to appsettings
                string guid = Guid.NewGuid().ToString("N");

                user.TelegramToken = guid;
                user.TelegramTokenGenDate = DateTime.UtcNow;

                await _unitOfWork.CommitAsync();

                string link = $"https://t.me/whatnotification_bot?start={guid}";

                result = Result<string>.GetSuccess(link);
            }
            else
            {
                result = Result<string>.GetError(ErrorCode.Conflict,
                    "Telegram account is already synched");
            }

            return result;
        }

        public async Task<Result<AccountDto>> SynchronizeTelegramAccount(string telegramToken, string telegramId)
        {
            var user = await _unitOfWork.AccountRepository
                .GetAccountByTelegramToken(telegramToken);

            Result<AccountDto> result;

            if (user == null)
            {
                result = Result<AccountDto>.GetError(ErrorCode.NotFound,
                    "Account not found");
            }
            else if (DateTime.UtcNow - user.TelegramTokenGenDate
                > _telegramTokenValidTime)
            {
                result = Result<AccountDto>.GetError(ErrorCode.Forbidden,
                    "Telegram token expired");
            }
            else
            {
                user.TelegramId = telegramId;
                user.TelegramToken = string.Empty;
                user.TelegramTokenGenDate = null;

                await _unitOfWork.CommitAsync();

                result = Result<AccountDto>.GetSuccess(_mapper.Map<AccountDto>(user));
            }

            return result;
        }

        public async Task<bool> ClearOldTelegramTokens()
        {
            var accounts = await _unitOfWork.AccountRepository
                .GetAllAccountsWithTelegramTokens();

            foreach (var account in accounts)
            {
                if (DateTime.UtcNow - account.TelegramTokenGenDate
                    > _telegramTokenValidTime)
                {
                    account.TelegramToken = null;
                    account.TelegramTokenGenDate = null;
                }
            }

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<Result<AccountDto>> GetAccountByTelegramIdAsync(string telegramId)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountByTelegramId(telegramId);

            Result<AccountDto> result;

            if (account != null)
            {
                var foundAccount = _mapper.Map<AccountDto>(account);

                result = Result<AccountDto>.GetSuccess(foundAccount);
            }
            else
            {
                result = Result<AccountDto>.GetError(ErrorCode.NotFound, "User with current telegram id not found");
            }

            return result;

        }

        public async Task<Account> GetAccountByTelegramId(long telegramId)
        {
            return await _unitOfWork.AccountRepository.GetAccountByTelegramId(telegramId);
        }

        //ToDo: Add realisation (I think we need this implementation in a new issue in a separate branch)
        public async Task<Result<bool>> DeleteAccountSyncronizationAsync(string telegramId)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountByTelegramId(telegramId);

            //need to add check (user can delete only own sync with Telegram)
            //if(account?.TelegramId == _currentUserService.TelegramId)
            //{
                  
            //}
            //else
            //{
            //    return Result<bool>.GetError(ErrorCode.Forbidden, "User can delete only own telegram synchronization");
            //}

            await _unitOfWork.CommitAsync();
            throw new NotImplementedException();
        }
    }
}
