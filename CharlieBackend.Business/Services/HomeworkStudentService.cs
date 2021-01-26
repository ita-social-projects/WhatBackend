using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class HomeworkStudentService : IHomeworkStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeworkStudentService> _logger;

        public HomeworkStudentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HomeworkStudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<HomeworkStudentDto>> CreateHomeworkFromStudentAsync(HomeworkStudentRequestDto homeworkStudent, ClaimsPrincipal userContext)
        {
            var errors = await ValidateHomeworStudentRequest(homeworkStudent).ToListAsync();

            if (errors.Any())
            {
                var errorsList = string.Join("; ", errors);

                _logger.LogError("Homework create request has failed due to: " + errorsList);

                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, errorsList);
            }

            long accountId = Convert.ToInt32(userContext.Claims.First(x => x.Type.EndsWith("AccountId")).Value);

            var student = await _unitOfWork.StudentRepository.GetStudentByAccountIdAsync(accountId);
            var studentGroups = await _unitOfWork.StudentGroupRepository.GetStudentGroupsByStudentId(student.Id);
            var lesson = await _unitOfWork.LessonRepository.GetLessonByHomeworkId(homeworkStudent.HomeworkId);

            if (!studentGroups.Contains(lesson.StudentGroupId.Value))
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, $"Student with {accountId} Id number not include in student group which have been lesson with {lesson.Id} Id number");
            }

            var homework = await _unitOfWork.HomeworkRepository.GetByIdAsync(homeworkStudent.HomeworkId);

            if (homework.DueDate < DateTime.Now)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, $"Due date already finished. Due date {homework.DueDate}");
            }

            var newHomework = new HomeworkStudent
            {
                StudentId = student.Id,
                HomeworkId = homework.Id,
                Homework = homework,
                HomeworkText = homeworkStudent.HomeworkText
            };

            _unitOfWork.HomeworkStudentRepository.Add(newHomework);

            if (homeworkStudent.AttachmentIds?.Count > 0)
            {
                var attachments = await _unitOfWork.AttachmentRepository.GetAttachmentsByIdsAsync(homeworkStudent.AttachmentIds);

                newHomework.AttachmentOfHomeworkStudents = new List<AttachmentOfHomeworkStudent>();

                foreach (var attachment in attachments)
                {
                    newHomework.AttachmentOfHomeworkStudents.Add(new AttachmentOfHomeworkStudent
                    {
                        AttachmentId = attachment.Id,
                        Attachment = attachment,
                    });
                }
            }

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"Homework with id {newHomework.Id} for student with id {accountId} has been added ");

            return Result<HomeworkStudentDto>.GetSuccess(_mapper.Map<HomeworkStudentDto>(newHomework));
        }

        public async Task<IList<HomeworkStudentDto>> GetHomeworkStudentForStudent(ClaimsPrincipal userContext)
        {
            long accountId = Convert.ToInt32(userContext.Claims.First(x => x.Type.EndsWith("AccountId")).Value);

            var student = await _unitOfWork.StudentRepository.GetStudentByAccountIdAsync(accountId);
            var homework = await _unitOfWork.HomeworkStudentRepository.GetHomeworkStudentForStudentByStudentId(student.Id);

            return _mapper.Map<IList<HomeworkStudentDto>>(homework);
        }

        public async Task<IList<HomeworkStudentDto>> GetHomeworkStudentForMentorByHomeworkId(long homeworkId ,ClaimsPrincipal userContext)
        {
            long accountId = Convert.ToInt32(userContext.Claims.First(x => x.Type.EndsWith("AccountId")).Value);

            var mentor = await _unitOfWork.MentorRepository.GetMentorByAccountIdAsync(accountId);
            var homework = await _unitOfWork.HomeworkStudentRepository.GetHomeworkStudentForMentorByHomeworkId(homeworkId);

            return _mapper.Map<IList<HomeworkStudentDto>>(homework);
        }

        private async IAsyncEnumerable<string> ValidateHomeworStudentRequest(HomeworkStudentRequestDto request)
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
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
