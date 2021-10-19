using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Business.Services.Notification.Interfaces;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Exceptions;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IHangfireJobService _jobService;

        public HomeworkService(IUnitOfWork unitOfWork, ICourseService courseService, ILessonService lessonService, IMapper mapper,
            ILogger<HomeworkService> logger, ICurrentUserService currentUserService, IHangfireJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _courseService = courseService;
            _lessonService = lessonService;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
            _jobService = jobService;
        }

        public async Task<Result<IList<HomeworkDto>>> GetHomeworks()
        {
            var homeworks = await _unitOfWork.HomeworkRepository.GetHomeworksWithThemeNameAndAtachemntsQuery().Select(x => new HomeworkDto
            {
                ThemeName = x.Lesson.Theme.Name,
                CreatedBy = x.CreatedBy,
                DueDate = x.DueDate,
                Id = x.Id,
                LessonId = x.LessonId,
                PublishingDate = x.PublishingDate,
                TaskText = x.TaskText,
                AttachmentIds = x.AttachmentsOfHomework.Select(a => a.AttachmentId).ToList()
            }).ToListAsync();

            return Result<IList<HomeworkDto>>.GetSuccess(homeworks);
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

            await _jobService.CreateAddHomeworkJob(newHomework);

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

        public async IAsyncEnumerable<string> HasMentorRightsToSeeHomeworks(GetHomeworkRequestDto request)
        {
            if (request.GroupId.HasValue && !await _unitOfWork.StudentGroupRepository.DoesMentorHaveAccessToGroup(_currentUserService.EntityId, request.GroupId.Value))
            {
                yield return "Mentor can get only homeworks of groups of his courses";
            }

            if (request.CourseId.HasValue && !await _unitOfWork.CourseRepository.DoesMentorHasAccessToCourse(_currentUserService.EntityId, request.CourseId.Value))
            {
                yield return "Mentor can get only homeworks of his courses";
            }
        }

        public async Task<Result<IList<HomeworkDto>>> GetHomeworksAsync(GetHomeworkRequestDto request)
        {
            IList<Homework> homeworks = null;
            if (_currentUserService.Role.Is(UserRole.Mentor))
            {
                var hasRightsError = await HasMentorRightsToSeeHomeworks(request).ToListAsync();
                if (hasRightsError.Any())
                {
                    _logger.LogInformation("Getting homeworks has failed due to mentor's rights.");
                    return Result<IList<HomeworkDto>>.GetError(ErrorCode.ValidationError, hasRightsError);
                }

                homeworks = await _unitOfWork.HomeworkRepository.GetHomeworksForMentor(request, _currentUserService.EntityId);
            }

            if (_currentUserService.Role.Is(UserRole.Student))
            {
                homeworks = await _unitOfWork.HomeworkRepository
                    .GetHomeworksForStudent(request, _currentUserService.EntityId);
            }
            
            if(_currentUserService.Role.Is(UserRole.Admin) || _currentUserService.Role.Is(UserRole.Secretary)) 
            { 
                homeworks = await _unitOfWork.HomeworkRepository.GetHomeworks(request);
            }

            return Result<IList<HomeworkDto>>.GetSuccess(_mapper.Map<IList<HomeworkDto>>(homeworks));
        }

        public async Task<Result<IList<HomeworkDto>>> GetHomeworkNotDone(long studentGroupId, DateTime? dueDate)
        {
            if (dueDate != null)
            {
                dueDate = dueDate.Value.Date.AddDays(1);
            }

            var studentId = _currentUserService.EntityId;

            if (!await _unitOfWork.StudentGroupRepository.DoesStudentBelongsGroup(studentId,studentGroupId))
            {
                return Result<IList<HomeworkDto>>.GetError(ErrorCode.NotFound, "Group id is incorrect");
            }

            var homeworks = await _unitOfWork.HomeworkRepository.GetNotDoneHomeworksByStudentGroup(studentGroupId, studentId, dueDate);

            return Result<IList<HomeworkDto>>.GetSuccess(_mapper.Map<IList<HomeworkDto>>(homeworks));
        }

        public async Task<Result<HomeworkDto>> UpdateHomeworkAsync(long homeworkId, HomeworkRequestDto updateHomeworkDto)
        {
            var errors = await ValidateHomeworkRequest(updateHomeworkDto).ToListAsync();

            if (errors.Any())
            {
                var errorsList = string.Join(";", errors);

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

            await _jobService.CreateUpdateHomeworkJob(foundHomework);

            return Result<HomeworkDto>.GetSuccess(_mapper.Map<HomeworkDto>(foundHomework));
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

        public async Task<Result<VisitDto>> UpdateMarkAsync(UpdateMarkRequestDto request)
        {
            var visit = await _unitOfWork
                                  .LessonRepository
                                  .GetVisitByStudentHomeworkIdAsync(request
                                      .StudentHomeworkId);

            if (visit is null)
            {
                throw new NotFoundException($"Visit related to student howework with id {request.StudentHomeworkId} not found");
            }

            visit.StudentMark = (sbyte)request.StudentMark;

            _unitOfWork.VisitRepository.Update(visit);
            await _unitOfWork.CommitAsync();

            return Result<VisitDto>.GetSuccess(_mapper.Map<VisitDto>(visit));
        }
    }
}
