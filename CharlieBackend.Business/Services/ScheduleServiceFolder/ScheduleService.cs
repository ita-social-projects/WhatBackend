using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.ScheduleServiceFolder;

namespace CharlieBackend.Business.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IScheduledEventHandlerFactory _scheduledEventFactory;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IScheduledEventHandlerFactory scheduledEventHandlerFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduledEventFactory = scheduledEventHandlerFactory;
        }

        public async Task<Result<EventOccurrenceDTO>> CreateScheduleAsync(CreateScheduleDto createScheduleRequest)
        {
            string error = await ValidateCreateScheduleRequestAsync(createScheduleRequest);

            if (error != null)
            {
                return Result<EventOccurrenceDTO>.GetError(ErrorCode.ValidationError, error);
            }

            EventOccurrence result = new EventOccurrence
            {
                Pattern = createScheduleRequest.Pattern.Type,
                StudentGroupId = createScheduleRequest.Context.GroupID,
                EventStart = createScheduleRequest.Range.StartDate,
                EventFinish = createScheduleRequest.Range.FinishDate.Value,
                Storage = EventOccuranceStorageParser.GetPatternStorageValue(createScheduleRequest.Pattern)
            };

            _unitOfWork.EventOccurenceRepository.Add(result);

            _unitOfWork.ScheduledEventRepository.AddRange(_scheduledEventFactory.Get(createScheduleRequest.Pattern).GetEvents(result, createScheduleRequest.Context));

            await _unitOfWork.CommitAsync();

            return Result<EventOccurrenceDTO>.GetSuccess(_mapper.Map<EventOccurrenceDTO>(result));
        }       

        public async Task<Result<EventOccurrenceDTO>> DeleteScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.EventOccurenceRepository.GetByIdAsync(id);

            if (scheduleEntity != null)
            {
                var mappedSchedule = _mapper.Map<EventOccurrenceDTO>(scheduleEntity);
                await _unitOfWork.EventOccurenceRepository.DeleteAsync(id);

                await _unitOfWork.CommitAsync();

                return Result<EventOccurrenceDTO>.GetSuccess(mappedSchedule);
            }

            return Result<EventOccurrenceDTO>.GetError(ErrorCode.NotFound,
                $"Schedule with id={id} does not exist");
        }

        public async Task<Result<IList<EventOccurrenceDTO>>> GetAllSchedulesAsync()
        {
            var scheduleEntities = await _unitOfWork.EventOccurenceRepository.GetAllAsync();

            return Result<IList<EventOccurrenceDTO>>.GetSuccess(
                _mapper.Map<IList<EventOccurrenceDTO>>(scheduleEntities));
        }

        public async Task<Result<EventOccurrenceDTO>> GetScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.EventOccurenceRepository.GetByIdAsync(id);

            return scheduleEntity == null ?
                Result<EventOccurrenceDTO>.GetError(ErrorCode.NotFound, $"Schedule with id={id} does not exist") :
                Result<EventOccurrenceDTO>.GetSuccess(_mapper.Map<EventOccurrenceDTO>(scheduleEntity));
        }

        public async Task<Result<IList<EventOccurrenceDTO>>> GetSchedulesByStudentGroupIdAsync(long id)
        {
            var groupEntity = await _unitOfWork.StudentGroupRepository.GetByIdAsync(id);

            if (groupEntity == null)
            {
                return null;
            }
            var schedulesOfGroup = await _unitOfWork.EventOccurenceRepository.GetSchedulesByStudentGroupIdAsync(id);

            return Result<IList<EventOccurrenceDTO>>.GetSuccess(
                _mapper.Map<IList<EventOccurrenceDTO>>(schedulesOfGroup));
        }

        public async Task<Result<EventOccurrenceDTO>> UpdateStudentGroupAsync(long scheduleId, UpdateScheduleDto scheduleDTO)
        {
            try
            {
                if (scheduleDTO == null)
                {
                    return Result<EventOccurrenceDTO>.GetError(ErrorCode.UnprocessableEntity, "UpdateScheduleDto is null");
                }
                var foundSchedule = await _unitOfWork.EventOccurenceRepository.GetByIdAsync(scheduleId);

                if (foundSchedule == null)
                {
                    return Result<EventOccurrenceDTO>.GetError(ErrorCode.NotFound,
                        $"Schedule with id={scheduleId} does not exist");
                }
                var updatedEntity = _mapper.Map<EventOccurrence>(scheduleDTO);

                var errorMessage = Validate(updatedEntity);

                if (errorMessage != null)
                {
                    Result<EventOccurrenceDTO>.GetError(ErrorCode.ValidationError, errorMessage);
                }

                foundSchedule.Pattern = updatedEntity.Pattern;

                foundSchedule.Storage = updatedEntity.Storage;

                await _unitOfWork.CommitAsync();

                return Result<EventOccurrenceDTO>.GetSuccess(_mapper.Map<EventOccurrenceDTO>(foundSchedule));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<EventOccurrenceDTO>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        private bool IsNotValidRepeating(EventOccurrence entity)
        {
            return entity.Pattern != PatternType.Daily &&
                entity.Pattern != default;
        }

        private string Validate(EventOccurrence schedule)
        {

            if (IsNotValidRepeating(schedule))
            {
                return "DayNumber can`t be null because RepeatRate is not either 0 or 1";
            }

            if (schedule.EventFinish <= schedule.EventStart)
            {
                return "LessonEnd can`t be less than or equal to LessonStart";
            }

            return null;
        }

        private async Task<string> ValidateCreateScheduleRequestAsync(CreateScheduleDto request)
        {
            if (request == null)
            {
                return "Request must not be null";
            }

            string error = null;

            if (request.Pattern.Interval <= 0)
            {
                error = "Interval value out of range";
            }

            if (!await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(request.Context.GroupID))
            {
                error = "Group does not exist";
            }

            if (request.Context.MentorID.HasValue && !await _unitOfWork.MentorRepository.IsEntityExistAsync(request.Context.MentorID.Value))
            {
                error = "Mentor does not exist";
            }

            if (request.Context.ThemeID.HasValue && !await _unitOfWork.ThemeRepository.IsEntityExistAsync(request.Context.ThemeID.Value))
            {
                error = "Theme does not exist";
            }

            switch (request.Pattern.Type)
            {
                case PatternType.Daily:
                    break;
                case PatternType.Weekly:
                    if (request.Pattern.DaysOfWeek == null || request.Pattern.DaysOfWeek.Count == 0)
                    {
                        error = "Target days not provided";
                    }
                    break;
                case PatternType.AbsoluteMonthly:
                    if (request.Pattern.Dates == null || request.Pattern.Dates.Count == 0)
                    {
                        error = "Target dates not provided";
                    }
                    break;
                case PatternType.RelativeMonthly:
                    if ((request.Pattern.DaysOfWeek == null || request.Pattern.DaysOfWeek.Count == 0) 
                        || (request.Pattern.Index != null ? request.Pattern.Index <= 0 : false))
                    {
                        error = "Target days not provided";
                    }
                    break;
                default:
                    error = "Pattern type not supported";
                    break;
            }

            return error;
        }
    }
}
