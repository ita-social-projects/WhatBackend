using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class EventsService : IEventsService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IScheduledEventHandlerFactory _scheduledEventFactory;
        private readonly SchedulesEventsValidator _validator;

        public EventsService(IUnitOfWork unitOfWork, IMapper mapper, IScheduledEventHandlerFactory scheduledEventHandlerFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduledEventFactory = scheduledEventHandlerFactory;
            _validator = new SchedulesEventsValidator(unitOfWork);
        }

        public async Task<Result<bool>> DeleteConcreteScheduleByIdAsync(long id)
        {
            if (id < 0)
            {
                return Result<bool>.GetError(ErrorCode.Conflict,
                    "Can not delete scheduled event due to wrong request data");
            }

            var scheduledEvent = await _unitOfWork.ScheduledEventRepository.GetByIdAsync(id);

            if (scheduledEvent is null)
            {
                return Result<bool>.GetError(ErrorCode.ValidationError, "Scheduled event does not exist");
            }

            await _unitOfWork.ScheduledEventRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Result<bool>.GetSuccess(true);
        }

        public async Task<Result<ScheduledEventDTO>> UpdateScheduledEventByID(long scheduledEventId, UpdateScheduledEventDto request)
        {
            string error = await _validator.ValidateScheduledEventId(scheduledEventId);

            error ??= await _validator.ValidateUpdateScheduleDTO(request);

            if (error != null)
            {
                return Result<ScheduledEventDTO>.GetError(ErrorCode.ValidationError, error);
            }

            ScheduledEvent item = await _unitOfWork.ScheduledEventRepository.GetByIdAsync(scheduledEventId);

            item = SchedulesUpdater.UpdateFields(item, request);

            _unitOfWork.ScheduledEventRepository.Update(item);

            await _unitOfWork.CommitAsync();

            return Result<ScheduledEventDTO>.GetSuccess(_mapper.Map<ScheduledEventDTO>(item));
        }

        public async Task<Result<ScheduledEventDTO>> GetConcreteScheduleByIdAsync(long eventId)
        {
            var foundScheduleEvent = await _unitOfWork.ScheduledEventRepository.GetByIdAsync(eventId);

            return foundScheduleEvent == null ?
                Result<ScheduledEventDTO>.GetError(ErrorCode.NotFound, $"Single schedule event with id={eventId} does not exist") :
                Result<ScheduledEventDTO>.GetSuccess(_mapper.Map<ScheduledEventDTO>(foundScheduleEvent));
        }
    }
}
