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

        public async Task<Result<ScheduleDto>> CreateScheduleAsync(CreateScheduleDto scheduleModel)
        {
            try
            {
                if (scheduleModel == null)
                {
                    return Result<ScheduleDto>.Error(ErrorCode.ValidationError, "ScheduleDto is null");
                }
                if(scheduleModel.RepeatRate != RepeatRate.Daily && scheduleModel.RepeatRate != RepeatRate.Never && 
                    scheduleModel.DayNumber == null)
                {
                    Result<ScheduleDto>.Error(ErrorCode.ValidationError, "DayNumber can`t be null"); 
                }

                var scheduleEntity = _mapper.Map<Schedule>(scheduleModel);
                scheduleEntity.StudentGroup = await _unitOfWork.StudentGroupRepository
                    .GetByIdAsync(scheduleEntity.Id);

                if(scheduleEntity.StudentGroup == null)
                {
                    return Result<ScheduleDto>.Error(ErrorCode.NotFound, "StudentGroupId is not valid");         
                }

                _unitOfWork.ScheduleRepository.Add(scheduleEntity);
             
                await _unitOfWork.CommitAsync();

                return Result<ScheduleDto>.Success(_mapper.Map<ScheduleDto>(scheduleEntity));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<ScheduleDto>.Error(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public async Task<IList<ScheduleDto>> GetAllSchedulesAsync()
        {
            var scheduleEntities = await _unitOfWork.ScheduleRepository.GetAllAsync();
            return _mapper.Map<IList<Schedule>, IList<ScheduleDto>>(scheduleEntities);
        }

        public async Task<Result<ScheduleDto>> GetScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.ScheduleRepository.GetByIdAsync(id);
            return scheduleEntity == null ? 
                Result<ScheduleDto>.Error(ErrorCode.NotFound, "Schedule id is not valid") :
                Result<ScheduleDto>.Success(_mapper.Map<ScheduleDto>(scheduleEntity));
        }

        public async Task<IList<ScheduleDto>> GetSchedulesByStudentGroupIdAsync(long id)
        {
            var groupEntity = await _unitOfWork.StudentGroupRepository.GetByIdAsync(id);
            if(groupEntity == null)
            {
               return null;
            }
            var schedulesOfGroup = await _unitOfWork.ScheduleRepository.GetSchedulesByStudentGroupIdAsync(id);
            return _mapper.Map<IList<Schedule>, IList<ScheduleDto>>(schedulesOfGroup);
        }

        public async Task<Result<UpdateScheduleDto>> UpdateStudentGroupAsync(UpdateScheduleDto scheduleModel)
        {
            throw new NotImplementedException();
        }
    }
}
