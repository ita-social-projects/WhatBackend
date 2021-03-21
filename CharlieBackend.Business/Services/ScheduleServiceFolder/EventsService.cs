using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Business.Exceptions;
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

        public EventsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> DeleteAsync(long id)
        {
            ScheduledEvent scheduledEvent = await GetEventAsync(id);

            await _unitOfWork.ScheduledEventRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Result<bool>.GetSuccess(true);
        }

        public async Task<ScheduledEventDTO> UpdateAsync(long id, UpdateScheduledEventDto request)
        {
            ScheduledEvent item = await GetEventAsync(id);

            item = SchedulesUpdater.UpdateFields(item, request);
            _unitOfWork.ScheduledEventRepository.Update(item);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ScheduledEventDTO>(item);
        }

        public async Task<ScheduledEventDTO> GetAsync(long id)
        {
            var foundScheduleEvent = await GetEventAsync(id);
            return _mapper.Map<ScheduledEventDTO>(foundScheduleEvent);
        }

        private async Task<ScheduledEvent> GetEventAsync(long id)
        {
            var foundScheduleEvent = await _unitOfWork.ScheduledEventRepository.GetByIdAsync(id);

            if (foundScheduleEvent == null)
            {
                throw new NotFoundException(ExceptionsConstants.EventNotFound);
            }
            return foundScheduleEvent;
        }
    }
}
