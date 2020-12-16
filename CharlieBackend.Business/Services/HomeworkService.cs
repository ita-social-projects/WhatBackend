using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeworkService> _logger;

        public HomeworkService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HomeworkService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<HometaskDto>> CreateHometaskAsync(CreateHometaskDto createHometaskDto)
        {
            try
            {
                var errors = ValidateCreateHometaskRequest(createHometaskDto);
                
                if (await errors.AnyAsync())
                {
                    return Result<HometaskDto>.GetError(ErrorCode.ValidationError, string.Join(";\n", errors));
                }

                var newHometask = new Hometask
                {
                    Comment = createHometaskDto.Comment,
                    Common = createHometaskDto.Common,
                    DeadlineDays = createHometaskDto.DeadlineDays,
                    MentorId = createHometaskDto.MentorId,
                    TaskText = createHometaskDto.TaskText,
                    ThemeId = createHometaskDto.ThemeId,
                };

                if (createHometaskDto.AttachmentIds?.Count != null)
                {
                    var attachments = await _unitOfWork.AttachmentRepository.GetAttachmentsByIdsAsync(createHometaskDto.AttachmentIds);
                    newHometask.AttachmentOfHometasks = new List<AttachmentOfHometask>();

                    foreach (var attachment in attachments) 
                    {
                        newHometask.AttachmentOfHometasks.Add(new AttachmentOfHometask
                        {
                            AttachmentId = attachment.Id,
                            Attachment = attachment,
                        });
                    }
                }

                _unitOfWork.HometaskRepository.Add(newHometask);

                await _unitOfWork.CommitAsync();

                return Result<HometaskDto>.GetSuccess(_mapper.Map<HometaskDto>(newHometask));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<HometaskDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public async Task<Result<IList<HometaskDto>>> GetHometaskOfCourseAsync(long courseId)
        {
            if (courseId == default)
            {
                return Result<IList<HometaskDto>>
                    .GetError(ErrorCode.ValidationError, "Wrong course id");
            }

            var hometasks = await _unitOfWork.HometaskRepository
                .GetHometasksByCourseId(courseId);

            if (hometasks == default)
            {
                return Result<IList<HometaskDto>>.GetError(ErrorCode.NotFound, "Hometasks not found");
            }

            return Result<IList<HometaskDto>>.GetSuccess(_mapper.Map<IList<HometaskDto>>(hometasks));
        }

        public async Task<Result<HometaskDto>> GetHometaskByIdAsync(long hometaskId)
        {
            if (hometaskId == default)
            {
                return Result<HometaskDto>
                    .GetError(ErrorCode.ValidationError, "Wrong hometask id");
            }

            var hometask = await _unitOfWork.HometaskRepository
                .GetByIdAsync(hometaskId);

            if (hometask == default)
            {
                return Result<HometaskDto>.GetError(ErrorCode.NotFound, "Hometask not found");
            }

            return Result<HometaskDto>.GetSuccess(_mapper.Map<HometaskDto>(hometask));
        }

        private async IAsyncEnumerable<string> ValidateCreateHometaskRequest(CreateHometaskDto request)
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            var theme = await _unitOfWork.ThemeRepository
                .CheckThemeExistenceByIdAsync(request.ThemeId);

            if (theme == default)
            {
                yield return "Given theme does not exist";
            }
        }
    }
}
