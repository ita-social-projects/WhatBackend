using CharlieBackend.Business.Services.Interfaces;
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
using CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers;
using CharlieBackend.Business.Services.Notification.Interfaces;

namespace CharlieBackend.Business.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IScheduledEventHandlerFactory _scheduledEventFactory;
        private readonly ISchedulesEventsDbEntityVerifier _validator;
        private readonly IHangfireJobService _jobService;

        public ScheduleService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IScheduledEventHandlerFactory scheduledEventHandlerFactory,
            ISchedulesEventsDbEntityVerifier validator,
            ICurrentUserService currentUserService,
            IHangfireJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduledEventFactory = scheduledEventHandlerFactory;
            _validator = validator;
            _currentUserService = currentUserService;
            _jobService = jobService;
        }

        public async Task<Result<EventOccurrenceDTO>> CreateScheduleAsync(CreateScheduleDto createScheduleRequest)
        {
            string error = await _validator.ValidateCreateScheduleRequestAsync(createScheduleRequest);

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

            _unitOfWork.EventOccurrenceRepository.Add(result);

            await _unitOfWork.CommitAsync();

            await AddEventsAsync(result, createScheduleRequest);

            return Result<EventOccurrenceDTO>.GetSuccess(_mapper.Map<EventOccurrenceDTO>(result));
        }

        private async Task AddEventsAsync(EventOccurrence result, CreateScheduleDto createScheduleRequest)
        {
            var events = _scheduledEventFactory.Get(createScheduleRequest.Pattern).GetEvents(result, createScheduleRequest.Context).ToList();
            _unitOfWork.ScheduledEventRepository.AddRange(events);
            await _unitOfWork.CommitAsync();
            await _jobService.CreateAddScheduledEventsJob(events);
        }

        public async Task<Result<EventOccurrenceDTO>> DeleteScheduleByIdAsync(long id, DateTime? startDate, DateTime? finishDate)
        {
            string error = await _validator.ValidateEventOccuranceId(id);

            if (error != null)
            {
                return Result<EventOccurrenceDTO>.GetError(ErrorCode.ValidationError, error);
            }

            var eventOccurrenceResult = await _unitOfWork.EventOccurrenceRepository.GetByIdAsync(id);

            var everyValue = eventOccurrenceResult
                .ScheduledEvents
                .Where(x => startDate.HasValue && x.EventFinish >= startDate.Value)
                .Where(x => finishDate.HasValue && x.EventStart <= finishDate.Value)
                .ToList();

            var everyValueWithLesson = everyValue.Where(x => x.LessonId != null);

            var result = everyValue.Where(x => x.LessonId == null);
            if (startDate.HasValue)
            {
                if (eventOccurrenceResult.ScheduledEvents.Except(everyValue).Any() || everyValueWithLesson.Any())
                {
                    DateTime actualFinish = eventOccurrenceResult.EventFinish;

                    if (!finishDate.HasValue || finishDate.Value >= eventOccurrenceResult.EventFinish)
                    {
                        if (everyValueWithLesson.Any() && everyValueWithLesson.Last().EventFinish >= startDate.Value)
                        {
                            actualFinish = everyValueWithLesson.Last().EventFinish;
                        }
                        else
                        {
                            actualFinish = startDate.Value;
                        }

                        eventOccurrenceResult.EventFinish = actualFinish;

                        _unitOfWork.EventOccurrenceRepository.Update(eventOccurrenceResult);
                    }

                    _unitOfWork.ScheduledEventRepository.RemoveRange(result);
                }
            }
            else
            {
                await _unitOfWork.EventOccurrenceRepository.DeleteAsync(id);
            }

            await _unitOfWork.CommitAsync();

            if(result.Count() != 0)
            {
                await _jobService.DeleteScheduledEventsJob(result);
            }
            else
            {
                await _jobService.DeleteScheduledEventsJob(eventOccurrenceResult.ScheduledEvents);
            }

            return Result<EventOccurrenceDTO>.GetSuccess(_mapper.Map<EventOccurrenceDTO>(eventOccurrenceResult));
        }

        public async Task<Result<EventOccurrenceDTO>> GetEventOccurrenceByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.EventOccurrenceRepository.GetByIdAsync(id);

            return scheduleEntity == null ?
                Result<EventOccurrenceDTO>.GetError(ErrorCode.NotFound, $"Schedule with id={id} does not exist") :
                Result<EventOccurrenceDTO>.GetSuccess(_mapper.Map<EventOccurrenceDTO>(scheduleEntity));
        }

        public async Task<Result<IList<EventOccurrenceDTO>>> GetEventOccurrencesAsync()
        {
            var eventOccurences = await _unitOfWork.EventOccurrenceRepository.GetAllAsync();

            return Result<IList<EventOccurrenceDTO>>.GetSuccess(
                _mapper.Map<IList<EventOccurrenceDTO>>(eventOccurences));
        }

        public async Task<Result<IList<EventOccurrenceDTO>>> GetEventOccurrencesByGroupIdAsync(long studentGroupId)
        {
            var eventOccurences = await _unitOfWork.EventOccurrenceRepository.GetByStudentGroupIdAsync(studentGroupId);

            return Result<IList<EventOccurrenceDTO>>.GetSuccess(
                _mapper.Map<IList<EventOccurrenceDTO>>(eventOccurences));
        }

        public async Task<Result<IList<ScheduledEventDTO>>> GetEventsFiltered(ScheduledEventFilterRequestDTO request)
        {
            string error = await _validator.ValidateGetEventsFilteredRequest(request);

            if (error != null)
            {
                return Result<IList<ScheduledEventDTO>>.GetError(ErrorCode.ValidationError, error);
            }

            if (_currentUserService.Role == UserRole.Student)
            {
                if (_currentUserService.AccountId != request.StudentAccountID)
                {
                    return Result<IList<ScheduledEventDTO>>.GetError(ErrorCode.Unauthorized, "Student can see only own events");
                }
            }
            else if (_currentUserService.Role == UserRole.Mentor)
            {
                if (_currentUserService.EntityId != request.MentorID)
                {
                    return Result<IList<ScheduledEventDTO>>.GetError(ErrorCode.Unauthorized, "Mentor can see only own events");
                }
            }

            var schedulesOfGroup = await _unitOfWork.ScheduledEventRepository.GetEventsFilteredAsync(request);

            return Result<IList<ScheduledEventDTO>>.GetSuccess(
                _mapper.Map<IList<ScheduledEventDTO>>(schedulesOfGroup));
        }

        public async Task<Result<IList<ScheduledEventDTO>>> UpdateEventsRange(ScheduledEventFilterRequestDTO filter, UpdateScheduledEventDto request)
        {
            string error = await _validator.ValidateGetEventsFilteredRequest(filter);

            if (error != null)
            {
                return Result<IList<ScheduledEventDTO>>.GetError(ErrorCode.ValidationError, error);
            }

            var allRelatedEvents = await _unitOfWork.ScheduledEventRepository.GetEventsFilteredAsync(filter);

            var result = allRelatedEvents.Where(x => x.LessonId is null);

            foreach (ScheduledEvent item in result)
            {
                SchedulesUpdater.UpdateFields(item, request);
            }

            _unitOfWork.ScheduledEventRepository.UpdateRange(result);

            await _unitOfWork.CommitAsync();

            await _jobService.CreateUpdateScheduledEventsJob(result);

            return Result<IList<ScheduledEventDTO>>.GetSuccess(_mapper.Map<IList<ScheduledEventDTO>>(result));
        }

        public async Task<Result<EventOccurrenceDTO>> UpdateEventOccurrenceById(long eventOccurrenceId, CreateScheduleDto request)
        {
            string error = await _validator.ValidateCreateScheduleRequestAsync(request);
            error ??= await _validator.ValidateEventOccuranceId(eventOccurrenceId);

            if (error != null)
            {
                return Result<EventOccurrenceDTO>.GetError(ErrorCode.ValidationError, error);
            }

            var eventOccurrenceResult = await _unitOfWork.EventOccurrenceRepository.GetByIdAsync(eventOccurrenceId);

            eventOccurrenceResult.Pattern = request.Pattern.Type;
            eventOccurrenceResult.StudentGroupId = request.Context.GroupID;
            eventOccurrenceResult.EventStart = request.Range.StartDate;
            eventOccurrenceResult.EventFinish = request.Range.FinishDate.Value;
            eventOccurrenceResult.Storage = EventOccuranceStorageParser.GetPatternStorageValue(request.Pattern);

            var removedScheduledEvents = eventOccurrenceResult.ScheduledEvents.Where(x => x.LessonId is null).ToList();
            _unitOfWork.ScheduledEventRepository.RemoveRange(removedScheduledEvents);
            var events = _scheduledEventFactory.Get(request.Pattern).GetEvents(eventOccurrenceResult, request.Context).ToList();
            _unitOfWork.ScheduledEventRepository.AddRange(events);
            _unitOfWork.EventOccurrenceRepository.Update(eventOccurrenceResult);

            await _unitOfWork.CommitAsync();

            await _jobService.CreateUpdateEventOccurrenceJob(removedScheduledEvents, events);

            return Result<EventOccurrenceDTO>.GetSuccess(_mapper.Map<EventOccurrenceDTO>(eventOccurrenceResult));
        }

        async public Task<Result<DetailedEventOccurrenceDTO>> GetDetailedEventOccurrenceById(long eventOccurrenceId)
        {
            var eventOccurrence = (await GetEventOccurrenceByIdAsync(eventOccurrenceId)).Data;

            var detailedEventOccurrence = new DetailedEventOccurrenceDTO
            {
                Id = eventOccurrence.Id,
                Pattern = EventOccuranceStorageParser.GetFullDataFromStorage(eventOccurrence.Storage),
                Range = new OccurenceRange
                {
                    FinishDate = eventOccurrence.EventFinish,
                    StartDate = eventOccurrence.EventStart
                },
                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = eventOccurrence.StudentGroupId
                }
            };

            return Result<DetailedEventOccurrenceDTO>.GetSuccess(detailedEventOccurrence);
        }
    }
}
