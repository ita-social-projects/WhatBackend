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
                var scheduleEntity = _mapper.Map<Schedule>(scheduleModel);
                scheduleEntity.StudentGroup = _unitOfWork.StudentGroupRepository
                    .GetByIdAsync(scheduleEntity.Id).Result;
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

        public bool DeleteSchedule(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ScheduleDto>> GetAllSchedulesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<ScheduleDto>> GetScheduleByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ScheduleDto>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UpdateScheduleDto>> UpdateStudentGroupAsync(UpdateScheduleDto scheduleModel)
        {
            throw new NotImplementedException();
        }
    }
}
