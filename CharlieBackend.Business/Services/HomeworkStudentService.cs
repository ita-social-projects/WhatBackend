﻿using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Business.Services.Notification.Interfaces;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    /// <summary>
    /// this service is for creating homework from a student
    /// </summary>
    public class HomeworkStudentService : IHomeworkStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeworkStudentService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHomeworkService _homeworkService;
        private readonly IHangfireJobService _jobService;

        public HomeworkStudentService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<HomeworkStudentService> logger,
            ICurrentUserService currentUserService,
            IHomeworkService homeworkService,
            IHangfireJobService hangfireJobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
            _homeworkService = homeworkService;
            _jobService = hangfireJobService;
        }

        public async Task<Result<HomeworkStudentDto>> CreateHomeworkFromStudentAsync(HomeworkStudentRequestDto homeworkStudent)
        {
            if (homeworkStudent == default)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, $"Please provide request data");
            }

            long accountId = _currentUserService.AccountId;

            var student = await _unitOfWork.StudentRepository.GetStudentByAccountIdAsync(accountId);

            if (student == default)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.NotFound, $"Student with account id {accountId} was not found");
            }

            var homework = await _unitOfWork.HomeworkRepository.GetByIdAsync(homeworkStudent.HomeworkId);

            if (homework == default)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.NotFound, $"Homework with id {homeworkStudent.HomeworkId} was not found");
            }

            var errors = await ValidateAddingHomeworkStudentRequest(homeworkStudent, student, homework).ToListAsync();

            if (errors.Any())
            {
                var errorsList = string.Join("; ", errors);

                _logger.LogError("Homework create request has failed due to: " + errorsList);

                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, errorsList);
            }

            if (homework.DueDate < DateTime.UtcNow)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, $"Due date already finished. Due date {homework.DueDate}");
            }

            var newHomework = new HomeworkStudent
            {
                StudentId = student.Id,
                HomeworkId = homework.Id,
                Homework = homework,
                HomeworkText = homeworkStudent.HomeworkText,
                PublishingDate = DateTime.UtcNow,
                IsSent = homeworkStudent.IsSent
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

            await _jobService.CreateAddHomeworkStudentJob(newHomework);

            _logger.LogInformation($"Homework with id {newHomework.Id} for student with id {accountId} has been added ");

            return Result<HomeworkStudentDto>.GetSuccess(_mapper.Map<HomeworkStudentDto>(newHomework));
        }

        public async Task<Result<HomeworkStudentDto>> UpdateHomeworkFromStudentAsync(HomeworkStudentRequestDto homeworkStudent, long homeworkId)
        {

            long accountId = _currentUserService.AccountId;
            var student = await _unitOfWork.StudentRepository.GetStudentByAccountIdAsync(accountId);

            if (student == default)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.NotFound, $"Student with account id {accountId} was not found");
            }

            var homework = await _unitOfWork.HomeworkRepository.GetByIdAsync(homeworkStudent.HomeworkId);

            if (homework == default)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.NotFound, $"Homework with id {homeworkStudent.HomeworkId} was not found");
            }

            var errors = await ValidateUpdatingHomeworkStudentRequest(homeworkStudent, student, homework).ToListAsync();

            if (errors.Any())
            {
                var errorsList = string.Join("; ", errors);

                _logger.LogError("Homework create request has failed due to: " + errorsList);

                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, errorsList);
            }

            var foundStudentHomework = await _unitOfWork.HomeworkStudentRepository.GetByIdAsync(homeworkId);

            if (foundStudentHomework == null)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.NotFound, $"Homework with id {homeworkId} not Found");
            }

            if (foundStudentHomework.StudentId != student.Id)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, $"Sorry, but homework with id {foundStudentHomework.Id} is not created by this student");
            }

            if (homework.DueDate < DateTime.UtcNow)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.ValidationError, $"Due date already finished. Due date {homework.DueDate}");
            }

            if (foundStudentHomework.IsSent == true && foundStudentHomework.Mark == null)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.Forbidden, $"Homework with id {foundStudentHomework.Id} hasn't evaluated yet");
            }

            if (foundStudentHomework.IsSent == true)
            {
                var homeworkStudentHistory = new HomeworkStudentHistory()
                {
                    HomeworkText = foundStudentHomework.HomeworkText,
                    HomeworkStudent = foundStudentHomework,
                    Mark = foundStudentHomework.Mark,
                    PublishingDate = DateTime.UtcNow,
                };

                var AttachmentOfHomeworkStudentsHistory = foundStudentHomework.AttachmentOfHomeworkStudents?.Select(attachmentOfHomework => new AttachmentOfHomeworkStudentHistory()
                {
                    HomeworkStudentHistory = homeworkStudentHistory,
                    Attachment = attachmentOfHomework.Attachment,
                    AttachmentId = attachmentOfHomework.AttachmentId,
                    HomeworkStudentHistoryId = homeworkStudentHistory.Id,
                });
                homeworkStudentHistory.AttachmentOfHomeworkStudentsHistory = AttachmentOfHomeworkStudentsHistory?.ToList();

                _unitOfWork.HomeworkStudentHistoryRepository.Add(homeworkStudentHistory);
            }

            foundStudentHomework.HomeworkText = homeworkStudent.HomeworkText;
            foundStudentHomework.PublishingDate = DateTime.UtcNow;
            foundStudentHomework.IsSent = homeworkStudent.IsSent;
            foundStudentHomework.Mark = null;
            foundStudentHomework.AttachmentOfHomeworkStudents = new List<AttachmentOfHomeworkStudent>();

            var newAttachments = new List<AttachmentOfHomeworkStudent>();

            if (homeworkStudent.AttachmentIds.Count() > 0)
            {
                newAttachments = homeworkStudent.AttachmentIds.Select(x => new AttachmentOfHomeworkStudent
                {
                    HomeworkStudentId = foundStudentHomework.HomeworkId,
                    HomeworkStudent = foundStudentHomework,
                    AttachmentId = x

                }).ToList();

                _unitOfWork.HomeworkStudentRepository.UpdateManyToMany(foundStudentHomework.AttachmentOfHomeworkStudents, newAttachments);

                foundStudentHomework.AttachmentOfHomeworkStudents = newAttachments;
            }

            await _unitOfWork.CommitAsync();

            await _jobService.CreateUpdateHomeworkStudentJob(foundStudentHomework);

            return Result<HomeworkStudentDto>.GetSuccess(_mapper.Map<HomeworkStudentDto>(foundStudentHomework));
        }

        public async Task<IList<HomeworkStudentDto>> GetHomeworkStudentForStudent()
        {
            var homework = await _unitOfWork.HomeworkStudentRepository.GetHomeworkStudentForStudent(_currentUserService.EntityId);

            return _mapper.Map<IList<HomeworkStudentDto>>(homework);
        }

        public async Task<Result<IList<HomeworkStudentDto>>> GetHomeworkStudentForMentor(long homeworkId)
        {
            var homework = await _unitOfWork.HomeworkRepository.GetByIdAsync(homeworkId);
            var result = new List<HomeworkStudent>();
            var errors = await _homeworkService.HasMentorRightsToSeeHomeworks(new Core.DTO.Homework.GetHomeworkRequestDto()
            {
                CourseId = homework?.Lesson.StudentGroup.CourseId,
                GroupId = homework?.Lesson.StudentGroupId
            }).ToListAsync();

            if (errors.Any() || homework == null)
                return Result<IList<HomeworkStudentDto>>.GetError(ErrorCode.NotFound, $"Homework with id {homeworkId} not found or mentor doesn't have access to it");

            var homeworksStudent = await _unitOfWork.HomeworkStudentRepository.GetHomeworkStudentForMentor(homework.Id);
            

            foreach (var homeworkStudent in homeworksStudent)
            {
                var homeworkStudentHistory = await _unitOfWork.HomeworkStudentHistoryRepository.GetHomeworkStudentHistoryByHomeworkStudentId(homeworkStudent.Id);

                if (homeworkStudent.IsSent == true)
                {
                    result.Add(homeworkStudent);
                }

                if (homeworkStudent.IsSent == false && homeworkStudentHistory.Count > 0)
                {
                    var correctHomeworkStudent = homeworkStudentHistory.Last();

                    homeworkStudent.HomeworkText = correctHomeworkStudent.HomeworkText;
                    homeworkStudent.IsSent = true;
                    homeworkStudent.Mark = correctHomeworkStudent.Mark;
                    homeworkStudent.PublishingDate = correctHomeworkStudent.PublishingDate;
                    homeworkStudent.MarkId = correctHomeworkStudent.MarkId;
                    homeworkStudent.AttachmentOfHomeworkStudents = correctHomeworkStudent.AttachmentOfHomeworkStudentsHistory
                        ?.Select(attachment => new AttachmentOfHomeworkStudent
                        {
                            Attachment = attachment.Attachment,
                            AttachmentId = attachment.AttachmentId,
                            HomeworkStudent = homeworkStudent,
                            HomeworkStudentId = attachment.HomeworkStudentHistoryId
                        }).ToList();

                    result.Add(homeworkStudent);
                }
            }
            return Result<IList<HomeworkStudentDto>>.GetSuccess(_mapper.Map<IList<HomeworkStudentDto>>(result));
        }

        public async Task<IList<HomeworkStudentDto>> GetHomeworkStudentHistoryByHomeworkStudentId(long homeworkStudentId)
        {
            var homeworkStudent = await _unitOfWork.HomeworkStudentRepository.GetByIdAsync(homeworkStudentId);
            var homeworkStudentHistories = await _unitOfWork.HomeworkStudentHistoryRepository.GetHomeworkStudentHistoryByHomeworkStudentId(homeworkStudentId);
            var result = new List<HomeworkStudent>();

            result = homeworkStudentHistories?.Select(homeworkStudentHistory => new HomeworkStudent()
            {
                Id = homeworkStudent.Id,
                StudentId = homeworkStudent.StudentId,
                HomeworkId = homeworkStudent.HomeworkId,
                HomeworkText = homeworkStudentHistory.HomeworkText,
                MarkId = homeworkStudentHistory.MarkId,
                PublishingDate = homeworkStudentHistory.PublishingDate,
                IsSent = true,
                Mark = homeworkStudentHistory.Mark,
                Student = homeworkStudent.Student,
                AttachmentOfHomeworkStudents = homeworkStudentHistory.AttachmentOfHomeworkStudentsHistory?.Select(elem =>
                new AttachmentOfHomeworkStudent()
                {
                    Attachment = elem.Attachment,
                    AttachmentId = elem.AttachmentId,
                    HomeworkStudent = homeworkStudent,
                    HomeworkStudentId = elem.HomeworkStudentHistoryId
                }).ToList()
            }).ToList();

            if (homeworkStudent.IsSent == true)
            {
                result.Add(homeworkStudent);
            }

            return _mapper.Map<IList<HomeworkStudentDto>>(result);
        }

        public async Task<Result<HomeworkStudentDto>> UpdateMarkAsync(UpdateMarkRequestDto request)
        {
            var homeworkStudent = await _unitOfWork.HomeworkStudentRepository.GetByIdAsync(request.StudentHomeworkId);

            if (homeworkStudent == null)
            {
                return Result<HomeworkStudentDto>.GetError(ErrorCode.NotFound, $"Homework from student with id {request.StudentHomeworkId} hasn't found");
            }

            var homeworkStudentHistory = await _unitOfWork.HomeworkStudentHistoryRepository.GetHomeworkStudentHistoryByHomeworkStudentId(homeworkStudent.Id);
            
            if (homeworkStudent.IsSent == false && homeworkStudentHistory != null)
            {
                homeworkStudentHistory.Last().Mark.Value = request.StudentMark;
                homeworkStudentHistory.Last().Mark.Comment = request.MentorComment;
                homeworkStudentHistory.Last().Mark.EvaluationDate = DateTime.UtcNow;
                homeworkStudentHistory.Last().Mark.Type = request.MarkType;
                homeworkStudentHistory.Last().Mark.EvaluatedBy = _currentUserService.AccountId;

                _unitOfWork.HomeworkStudentHistoryRepository.Update(homeworkStudentHistory.Last());
            }
            else
            {
                if (homeworkStudent.Mark == null)
                {
                    homeworkStudent.Mark = new Mark();
                }
                homeworkStudent.Mark.Value = request.StudentMark;
                homeworkStudent.Mark.Comment = request.MentorComment;
                homeworkStudent.Mark.EvaluationDate = DateTime.UtcNow;
                homeworkStudent.Mark.Type = request.MarkType;
                homeworkStudent.Mark.EvaluatedBy = _currentUserService.AccountId;

                _unitOfWork.HomeworkStudentRepository.Update(homeworkStudent);
            }

            await _unitOfWork.CommitAsync();

            await _jobService.CreateUpdateMarkJob(homeworkStudent);

            return Result<HomeworkStudentDto>.GetSuccess(_mapper.Map<HomeworkStudentDto>(homeworkStudent));
        }

        private async IAsyncEnumerable<string> ValidateAddingHomeworkStudentRequest(HomeworkStudentRequestDto homeworkStudent, Student student, Homework homework)
        {

            if (await _unitOfWork.HomeworkStudentRepository.IsStudentHasHomeworkAsync(student.Id, homeworkStudent.HomeworkId))
            {
                yield return $"You already add homework for this Hometask {homeworkStudent.HomeworkId}";
                yield break;
            }

            var errors = await ValidateHomeworkStudentRequestBase(homeworkStudent, student, homework).ToListAsync();
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    yield return error;
                }
            }
        }

        private async IAsyncEnumerable<string> ValidateUpdatingHomeworkStudentRequest(HomeworkStudentRequestDto homeworkStudent, Student student, Homework homework)
        {

            if (!await _unitOfWork.HomeworkStudentRepository.IsStudentHasHomeworkAsync(student.Id, homeworkStudent.HomeworkId))
            {
                yield return $"There is no homework for this Hometask {homeworkStudent.HomeworkId}";
                yield break;
            }

            var errors = await ValidateHomeworkStudentRequestBase(homeworkStudent, student, homework).ToListAsync();
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    yield return error;
                }
            }
        }

        private async IAsyncEnumerable<string> ValidateHomeworkStudentRequestBase(HomeworkStudentRequestDto homeworkStudent, Student student, Homework homework)
        {

            var studentGroups = await _unitOfWork.StudentGroupRepository.GetStudentGroupsIdsByStudentId(student.Id);

            if (studentGroups == default)
            {
                yield return "Student has no student groups";
                yield break;
            }

            var lesson = await _unitOfWork.LessonRepository.GetLessonByHomeworkId(homeworkStudent.HomeworkId);

            if (lesson == default)
            {
                yield return "Please be sure that lesson exists";
                yield break;
            }

            if (!studentGroups.Contains(lesson.StudentGroupId.Value))
            {
                yield return $"Student with {student.Id} Id number not include in student group which have been lesson with {lesson.Id} Id number";
            }

            if (homeworkStudent.AttachmentIds?.Count() > 0)
            {
                var nonExistingAttachment = new List<long>();

                foreach (var attachmentId in homeworkStudent.AttachmentIds)
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
