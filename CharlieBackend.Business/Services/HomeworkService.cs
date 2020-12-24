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

        public async Task<Result<HomeworkDto>> CreateHomeworkAsync(HomeworkRequestDto createHomeworkDto)
        {
                var errors = await ValidateHomeworkRequest(createHomeworkDto)
                        .ToListAsync();
                
                if (errors.Any())
                {
                    var errorsList = string.Join("; ", errors);

                    _logger.LogError("Homework create request has failed due to: " + errorsList);

                    return Result<HomeworkDto>.GetError(ErrorCode.ValidationError, errorsList);
                }

                var newHomework = new Homework
                {
                    IsCommon = createHomeworkDto.IsCommon,
                    DeadlineDays = createHomeworkDto.DeadlineDays,
                    MentorId = createHomeworkDto.MentorId,
                    TaskText = createHomeworkDto.TaskText,
                    ThemeId = createHomeworkDto.ThemeId,
                };

                _unitOfWork.HomeworkRepository.Add(newHomework);

                if (createHomeworkDto.AttachmentIds?.Count > 0)
                {
                    var attachments = await _unitOfWork.AttachmentRepository.GetAttachmentsByIdsAsync(createHomeworkDto.AttachmentIds);

                    newHomework.AttachmentsOfHomework = new List<AttachmentOfHomework>();

                    foreach (var attachment in attachments) 
                    {
                    newHomework.AttachmentsOfHomework.Add(new AttachmentOfHomework
                        {
                            AttachmentId = attachment.Id,
                            Attachment = attachment,
                        });
                    }
                }

                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Homework with id {newHomework.Id} has been added");

                return Result<HomeworkDto>.GetSuccess(_mapper.Map<HomeworkDto>(newHomework));
        }

        public async Task<Result<IList<HomeworkDto>>> GetHomeworksOfCourseAsync(long courseId)
        {
            if (courseId == default)
            {
                return Result<IList<HomeworkDto>>
                    .GetError(ErrorCode.ValidationError, "Wrong course id");
            }

            var homeworks = await _unitOfWork.HomeworkRepository
                .GetHomeworksByCourseId(courseId);

            if (homeworks == default)
            {
                return Result<IList<HomeworkDto>>.GetError(ErrorCode.NotFound, "Homework not found");
            }

            return Result<IList<HomeworkDto>>.GetSuccess(_mapper.Map<IList<HomeworkDto>>(homeworks));
        }

        public async Task<Result<HomeworkDto>> GetHomeworkByIdAsync(long homeworkId)
        {
            if (homeworkId == default)
            {
                return Result<HomeworkDto>
                    .GetError(ErrorCode.ValidationError, "Wrong homework id");
            }

            var homework = await _unitOfWork.HomeworkRepository
                .GetByIdAsync(homeworkId);

            if (homework == default)
            {
                return Result<HomeworkDto>.GetError(ErrorCode.NotFound, "Homework not found");
            }

            return Result<HomeworkDto>.GetSuccess(_mapper.Map<HomeworkDto>(homework));
        }

        public async Task<Result<HomeworkDto>> UpdateHomeworkAsync(long homeworkId, HomeworkRequestDto updateHomeworkDto)
        {
            var errors = await ValidateHomeworkRequest(updateHomeworkDto).ToListAsync();

            if (errors.Any())
            {
                var errorsList = string.Join("; ", errors);

                _logger.LogError("Homework update request has failed due to: " + errorsList);

                return Result<HomeworkDto>.GetError(ErrorCode.ValidationError, errorsList);
            }

            var foundHomework = await _unitOfWork.HomeworkRepository.GetByIdAsync(homeworkId);

            if (foundHomework == null)
            {
                return Result<HomeworkDto>.GetError(ErrorCode.NotFound, "Given homework id not found");
            }

            foundHomework.DeadlineDays = updateHomeworkDto.DeadlineDays;
            foundHomework.IsCommon = updateHomeworkDto.IsCommon;
            foundHomework.TaskText = updateHomeworkDto.TaskText;

            if (foundHomework.MentorId != updateHomeworkDto.MentorId)
            {
                var mentor = await _unitOfWork.MentorRepository.GetByIdAsync(foundHomework.MentorId);

                foundHomework.MentorId = updateHomeworkDto.MentorId;
                foundHomework.Mentor = mentor;

            }

            if (foundHomework.ThemeId != updateHomeworkDto.ThemeId)
            {
                var theme = await _unitOfWork.ThemeRepository.GetByIdAsync(updateHomeworkDto.ThemeId);

                if (theme != default)
                {
                    foundHomework.ThemeId = updateHomeworkDto.ThemeId;
                    foundHomework.Theme = theme;
                }
            }

            if (updateHomeworkDto.AttachmentIds != null)
            {
                var newAttachments = updateHomeworkDto.AttachmentIds.Select(x => new AttachmentOfHomework
                {
                    HomeworkId = foundHomework.Id,
                    Homework = foundHomework,
                    AttachmentId = x
                });

                _unitOfWork.HomeworkRepository.UpdateManyToMany(foundHomework.AttachmentsOfHomework, newAttachments);
            }

            await _unitOfWork.CommitAsync();

            return Result<HomeworkDto>.GetSuccess(_mapper.Map<HomeworkDto>(foundHomework));
        }

        public async Task<Result<IList<HomeworkDto>>> GetHomeworksByThemeId(long themeId)
        {
            if (themeId == default)
            {
                return Result<IList<HomeworkDto>>.GetError(ErrorCode.ValidationError, "Wrong theme id");
            }

            var homeworks = await _unitOfWork.HomeworkRepository.GetHomeworksByThemeId(themeId);

            return Result<IList<HomeworkDto>>.GetSuccess(_mapper.Map<IList<HomeworkDto>>(homeworks));
        }

        private async IAsyncEnumerable<string> ValidateHomeworkRequest(HomeworkRequestDto request)
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            var theme = await _unitOfWork.ThemeRepository
                .IsEntityExistAsync(request.ThemeId);

            if (!theme)
            {
                yield return "Given theme does not exist";
            }

            var mentor = await _unitOfWork.MentorRepository
                .IsEntityExistAsync(request.MentorId);

            if (!mentor)
            {
                yield return "Given mentor does not exist";
            }

            if (request.AttachmentIds != default 
                && request.AttachmentIds.Count() != 0)
            {
                var nonExistingAttachment = new List<long>();

                foreach (var attachmentId in request.AttachmentIds)
                {
                    var doesAttachmentExist = await _unitOfWork.AttachmentRepository
                        .IsEntityExistAsync(attachmentId);

                    if (!doesAttachmentExist)
                    {
                        nonExistingAttachment.Add(attachmentId);
                    }
                }

                if (nonExistingAttachment.Count != 0)
                {
                    yield return "Given attachment ids do not exist: " + String.Join(", ", nonExistingAttachment);
                }
            }
        }
    }
}
