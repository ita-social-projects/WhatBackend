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

namespace CharlieBackend.Business.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<EventOccurenceDTO>> CreateScheduleAsync(CreateScheduleDto createScheduleRequest)
        {
            string error = ValidateCreateScheduleRequest(createScheduleRequest);

            if (error != null)
            {
                return Result<EventOccurenceDTO>.GetError(ErrorCode.ValidationError, error);
            }

            var result = CreateEventOccurence(createScheduleRequest);
            CreateSceduledEvents(result, createScheduleRequest.Context);

            await _unitOfWork.CommitAsync();

            return Result<EventOccurenceDTO>.GetSuccess(_mapper.Map<EventOccurenceDTO>(result));
        }

        private EventOccurence CreateEventOccurence(CreateScheduleDto createScheduleRequest)
        {
            EventOccurence result = new EventOccurence();

            result.Pattern = createScheduleRequest.Pattern.Type;
            result.StudentGroupId = createScheduleRequest.Context.GroupID;
            result.EventStart = createScheduleRequest.Range.StartDate;
            result.EventFinish = createScheduleRequest.Range.FinishDate.Value;
            result.Storage = EventOccuranceStorageParser.GetPatternStorageValue(createScheduleRequest.Pattern);

            _unitOfWork.EventOccurenceRepository.Add(result);

            return result;
        }

        private void CreateSceduledEvents(EventOccurence source, ContextForCreateScheduleDTO context)
        {
            IEnumerable<ScheduledEvent> result = ScheduledEventsHandler.GetEvents(source, context);

            _unitOfWork.ScheduledEventRepository.AddRange(result);
        }        

        public async Task<Result<EventOccurenceDTO>> DeleteScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.EventOccurenceRepository.GetByIdAsync(id);

            if (scheduleEntity != null)
            {
                var mappedSchedule = _mapper.Map<EventOccurenceDTO>(scheduleEntity);
                await _unitOfWork.EventOccurenceRepository.DeleteAsync(id);

                await _unitOfWork.CommitAsync();

                return Result<EventOccurenceDTO>.GetSuccess(mappedSchedule);
            }

            return Result<EventOccurenceDTO>.GetError(ErrorCode.NotFound,
                $"Schedule with id={id} does not exist");
        }

        public async Task<Result<IList<EventOccurenceDTO>>> GetAllSchedulesAsync()
        {
            var scheduleEntities = await _unitOfWork.EventOccurenceRepository.GetAllAsync();

            return Result<IList<EventOccurenceDTO>>.GetSuccess(
                _mapper.Map<IList<EventOccurenceDTO>>(scheduleEntities));
        }

        public async Task<Result<EventOccurenceDTO>> GetScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.EventOccurenceRepository.GetByIdAsync(id);

            return scheduleEntity == null ?
                Result<EventOccurenceDTO>.GetError(ErrorCode.NotFound, $"Schedule with id={id} does not exist") :
                Result<EventOccurenceDTO>.GetSuccess(_mapper.Map<EventOccurenceDTO>(scheduleEntity));
        }

        public async Task<Result<IList<EventOccurenceDTO>>> GetSchedulesByStudentGroupIdAsync(long id)
        {
            var groupEntity = await _unitOfWork.StudentGroupRepository.GetByIdAsync(id);

            if (groupEntity == null)
            {
                return null;
            }
            var schedulesOfGroup = await _unitOfWork.EventOccurenceRepository.GetSchedulesByStudentGroupIdAsync(id);

            return Result<IList<EventOccurenceDTO>>.GetSuccess(
                _mapper.Map<IList<EventOccurenceDTO>>(schedulesOfGroup));
        }

        public async Task<Result<EventOccurenceDTO>> UpdateStudentGroupAsync(long scheduleId, UpdateScheduleDto scheduleDTO)
        {
            try
            {

                if (scheduleDTO == null)
                {
                    return Result<EventOccurenceDTO>.GetError(ErrorCode.NotFound, "UpdateScheduleDto is null");
                }
                var foundSchedule = await _unitOfWork.EventOccurenceRepository.GetByIdAsync(scheduleId);

                if (foundSchedule == null)
                {
                    return Result<EventOccurenceDTO>.GetError(ErrorCode.NotFound,
                        $"Schedule with id={scheduleId} does not exist");
                }
                var updatedEntity = _mapper.Map<EventOccurence>(scheduleDTO);

                var errorMessage = Validate(updatedEntity);

                if (errorMessage != null)
                {
                    Result<EventOccurenceDTO>.GetError(ErrorCode.ValidationError, errorMessage);
                }

                foundSchedule.Pattern = updatedEntity.Pattern;

                if (updatedEntity.Storage != null)
                {
                    foundSchedule.Storage = updatedEntity.Storage;
                }
                await _unitOfWork.CommitAsync();

                return Result<EventOccurenceDTO>.GetSuccess(_mapper.Map<EventOccurenceDTO>(foundSchedule));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<EventOccurenceDTO>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        private bool IsNotValidRepeating(EventOccurence entity)
        {
            return entity.Pattern != PatternType.Daily &&
                entity.Pattern != default &&
                      entity.Storage == null;
        }

        private string Validate(EventOccurence schedule)
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

        private string ValidateCreateScheduleRequest(CreateScheduleDto request)
        {
            //TODO: validation for createScheduleDTO here
            return null;
        }
    }
}
