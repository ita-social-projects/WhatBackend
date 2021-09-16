using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Exceptions;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{

    /// <summary>
    /// this service is for creating homework from mentor for student 
    /// </summary>
    public class HomeworkService : IHomeworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseService _courseService;
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeworkService> _logger;
        private readonly ICurrentUserService _currentUserService;

        public HomeworkService(IUnitOfWork unitOfWork, ICourseService courseService, ILessonService lessonService, IMapper mapper, ILogger<HomeworkService> logger, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _courseService = courseService;
            _lessonService = lessonService;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
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

            var lesson = _unitOfWork.LessonRepository.GetByIdAsync(createHomeworkDto.LessonId);

            long accountId = _currentUserService.AccountId;

            var newHomework = new Homework
            {
                DueDate = createHomeworkDto.DueDate,
                LessonId = createHomeworkDto.LessonId,
                TaskText = createHomeworkDto.TaskText,
                Lesson = lesson.Result,
                PublishingDate = DateTime.UtcNow,
                CreatedBy = accountId
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

        public async Task<Result<IList<HomeworkDto>>> GetHomeworksByLessonId(long lessonId)
        {
            if (lessonId == default)
            {
                return Result<IList<HomeworkDto>>.GetError(ErrorCode.ValidationError, "Wrong lesson id");
            }

            var homeworks = await _unitOfWork.HomeworkRepository.GetHomeworksByLessonId(lessonId);

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

        public async IAsyncEnumerable<string> HasMentorRightsToSeeHomeworks(GetHomeworksRequestDto request)
        {
            if (request.CourseId != 0)
            {
                if (!await _unitOfWork.CourseRepository.IsEntityExistAsync((long)request.CourseId))
                {
                    yield return "Course does not exist";
                }
                else
                {
                    var courses = await _courseService
                        .CheckDoesMentorCanSeeCourseAsync(_currentUserService.EntityId, request.CourseId);
                    if (!courses)
                        yield return "Mentor can get only his courses";
                }
            }
            if (request.GroupId != 0)
            {
                if (!await _unitOfWork.StudentGroupRepository.IsEntityExistAsync((long)request.GroupId))
                {
                    yield return "Group does not exist";
                }
                else
                {
                    var groups = await _courseService
                        .CheckDoesMentorCanSeeGroupAsync(_currentUserService.EntityId, request.GroupId);
                    if (!groups)
                        yield return "Mentor can get only groups of his courses";
                }
            }
            if (request.ThemeId != 0)
            {
                if (!await _unitOfWork.ThemeRepository.IsEntityExistAsync((long)request.ThemeId))
                {
                    yield return "Theme does not exist";
                }
                else
                {
                    var groupsOfThemes = await _lessonService
                        .GetStudentGroupsOfThemes((long)request.ThemeId);
                    if (groupsOfThemes.Count == 0)
                        yield return "A lesson with this theme was not conducted yet";
                    else
                    {
                        List<long?> groups = new List<long?>();
                        foreach (var group in groupsOfThemes)
                        {
                            if (!await _courseService.CheckDoesMentorCanSeeGroupAsync(_currentUserService.EntityId, group))
                            {

                            }
                            else
                            {
                                groups.Add(group);
                            }
                        }
                        if (groups.Count == 0)
                            yield return "Mentor can not get homeworks with this theme";
                    }
                }
            }
        }

        public async Task<Result<IList<HomeworkDto>>> GetHomeworksForMentorByCourseIdAsync(long courseId)
        {
            var homeworksForMentor = await _unitOfWork.HomeworkRepository
                    .GetHomeworksForMentorByCourseId(courseId);
            return Result<IList<HomeworkDto>>.GetSuccess(_mapper.Map<IList<HomeworkDto>>(homeworksForMentor));
        }
        public async Task<Result<IList<HomeworkDto>>> GetHomeworksAsync(GetHomeworksRequestDto request)
        {
            var errors = ValidateGetHomeworksRequest(request);
            if (errors.Any())
            {
                return Result<IList<HomeworkDto>>.GetError(ErrorCode.ValidationError, string.Join(";\n", errors));
            }
            else
            {
                if (_currentUserService.Role.Is(UserRole.Mentor))
                {
                    var hasRightsError = await HasMentorRightsToSeeHomeworks(request).ToListAsync();
                    if (hasRightsError.Any())
                    {
                        var listOfErrors = string.Join("; ", hasRightsError);
                        _logger.LogError("Getting homeworks for mentor has failed due to: " + listOfErrors);
                        return Result<IList<HomeworkDto>>.GetError(ErrorCode.ValidationError, listOfErrors);
                    }
                }
                var homeworks = await _unitOfWork.HomeworkRepository
                    .GetHomeworks(request);
                return Result<IList<HomeworkDto>>.GetSuccess(_mapper.Map<IList<HomeworkDto>>(homeworks));
            }
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

            foundHomework.DueDate = updateHomeworkDto.DueDate;
            foundHomework.TaskText = updateHomeworkDto.TaskText;

            var newAttachments = new List<AttachmentOfHomework>();

            if (updateHomeworkDto.AttachmentIds?.Count() > 0)
            {
                newAttachments = updateHomeworkDto.AttachmentIds.Select(x => new AttachmentOfHomework
                {
                    HomeworkId = foundHomework.Id,
                    Homework = foundHomework,
                    AttachmentId = x
                }).ToList();
            }

            _unitOfWork.HomeworkRepository.UpdateManyToMany(foundHomework.AttachmentsOfHomework, newAttachments);

            await _unitOfWork.CommitAsync();

            return Result<HomeworkDto>.GetSuccess(_mapper.Map<HomeworkDto>(foundHomework));
        }

        private IEnumerable<string> ValidateGetHomeworksRequest(GetHomeworksRequestDto request)
        {
            if (request.GroupId == 0 && request.CourseId == 0
                && request.ThemeId == 0)
            {
                yield return "Please provide GroupId or CourseId or AccountId data";
            }
        }

        private async IAsyncEnumerable<string> ValidateHomeworkRequest(HomeworkRequestDto request)
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            var lesson = await _unitOfWork.LessonRepository.IsEntityExistAsync(request.LessonId);
               

            if (!lesson)
            {
                yield return "Given lesson does not exist";
            }

            if (request.AttachmentIds?.Count() > 0)
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
