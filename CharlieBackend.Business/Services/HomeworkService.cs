using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
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

            var lesson = _unitOfWork.LessonRepository.GetByIdAsync(createHomeworkDto.LessonId);

            var newHomework = new Homework
            {
                DueDate = createHomeworkDto.DueDate,
                LessonId = createHomeworkDto.LessonId,
                TaskText = createHomeworkDto.TaskText,
                Lesson = lesson.Result
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

            var studentHomework = await _unitOfWork.HomeworkStudentRepository
                .GetByIdAsync(request.StudentHomeworkId.GetValueOrDefault());

            if(studentHomework is null)
            {
                throw new NotFoundException("Student homework does not exist");
            }

            var homework = await _unitOfWork.HomeworkRepository.GetByIdAsync(studentHomework.HomeworkId);

            var visit = (await _unitOfWork.LessonRepository.GetByIdAsync(homework.LessonId))
                .Visits
                .FirstOrDefault(x => x.StudentId == studentHomework.StudentId);

            if (studentHomework is null)
            {
                throw new NotFoundException("Student visit does not exist");
            }

            visit.StudentMark = (sbyte)request.StudentMark;

            _unitOfWork.VisitRepository.Update(visit);
            await _unitOfWork.CommitAsync();

            return Result<VisitDto>.GetSuccess(_mapper.Map<VisitDto>(visit));
        }
    }
}
