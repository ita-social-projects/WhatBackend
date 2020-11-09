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

                if (IsNotValidRepeating(scheduleEntity))
                {
                    return Result<ScheduleDto>.Error(ErrorCode.ValidationError, "DayNumber can`t be null");
                }

                scheduleEntity.StudentGroup = await _unitOfWork.StudentGroupRepository
                    .GetByIdAsync(scheduleEntity.StudentGroupId);

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

        public async Task<Result<ScheduleDto>> DeleteScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.ScheduleRepository.GetByIdAsync(id);

            if(scheduleEntity != null)
            {
                var mappedSchedule = _mapper.Map<ScheduleDto>(scheduleEntity);
                await _unitOfWork.ScheduleRepository.DeleteAsync(id);

                await _unitOfWork.CommitAsync();

                return Result<ScheduleDto>.Success(mappedSchedule);
            }

            return Result<ScheduleDto>.Error(ErrorCode.NotFound, "scheduleId is not valid");
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

        public async Task<Result<ScheduleDto>> UpdateStudentGroupAsync(long scheduleId, UpdateScheduleDto scheduleDTO)
        {
            try
            {
                
                if(scheduleDTO == null)
                {
                    return Result<ScheduleDto>.Error(ErrorCode.NotFound, "UpdateScheduleDto is null");
                }
                var foundSchedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(scheduleId);

                if(foundSchedule == null)
                {
                    return Result<ScheduleDto>.Error(ErrorCode.NotFound, "Schedule id is not valid");
                }
                var updatedEntity = _mapper.Map<UpdateScheduleDto, Schedule>(scheduleDTO);

                if (IsNotValidRepeating(updatedEntity))
                {
                    Result<ScheduleDto>.Error(ErrorCode.ValidationError, "DayNumber can`t be null");
                }

                if(updatedEntity.LessonStart != default(TimeSpan))
                {
                    foundSchedule.LessonStart = updatedEntity.LessonStart;
                }

                if (updatedEntity.LessonEnd != default(TimeSpan))
                {
                    foundSchedule.LessonEnd = updatedEntity.LessonEnd;
                }

                foundSchedule.RepeatRate = updatedEntity.RepeatRate;

                if(updatedEntity.DayNumber != null)
                {
                    foundSchedule.DayNumber = updatedEntity.DayNumber;
                }
                await _unitOfWork.CommitAsync();

                return Result<ScheduleDto>.Success(_mapper.Map<ScheduleDto>(foundSchedule));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<ScheduleDto>.Error(ErrorCode.InternalServerError, "Internal error");
            }
        }

        private bool IsNotValidRepeating(Schedule entity)
        {
            return entity.RepeatRate != RepeatRate.Daily &&
                entity.RepeatRate != RepeatRate.Never &&
                      entity.DayNumber == null;
        }

    }
}
