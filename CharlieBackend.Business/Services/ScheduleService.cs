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
                    return Result<ScheduleDto>.GetError(ErrorCode.ValidationError, "ScheduleDto is null");
                }

                var scheduleEntity = _mapper.Map<Schedule>(scheduleModel);

                var errorMessage = Validate(scheduleEntity);

                if (errorMessage != null)
                {
                    return Result<ScheduleDto>.GetError(ErrorCode.ValidationError, errorMessage);
                }

                scheduleEntity.StudentGroup = await _unitOfWork.StudentGroupRepository
                    .GetByIdAsync(scheduleEntity.StudentGroupId);

                if (scheduleEntity.StudentGroup == null)
                {
                    return Result<ScheduleDto>.GetError(ErrorCode.NotFound,
                        $"Student group with id={scheduleModel.StudentGroupId} does not exist");
                }

                _unitOfWork.ScheduleRepository.Add(scheduleEntity);

                await _unitOfWork.CommitAsync();

                return Result<ScheduleDto>.GetSuccess(_mapper.Map<ScheduleDto>(scheduleEntity));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<ScheduleDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public async Task<Result<ScheduleDto>> DeleteScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.ScheduleRepository.GetByIdAsync(id);

            if (scheduleEntity != null)
            {
                var mappedSchedule = _mapper.Map<ScheduleDto>(scheduleEntity);
                await _unitOfWork.ScheduleRepository.DeleteAsync(id);

                await _unitOfWork.CommitAsync();

                return Result<ScheduleDto>.GetSuccess(mappedSchedule);
            }

            return Result<ScheduleDto>.GetError(ErrorCode.NotFound,
                $"Schedule with id={id} does not exist");
        }

        public async Task<Result<IList<ScheduleDto>>> GetAllSchedulesAsync()
        {
            var scheduleEntities = await _unitOfWork.ScheduleRepository.GetAllAsync();

            return Result<IList<ScheduleDto>>.GetSuccess(
                _mapper.Map<IList<ScheduleDto>>(scheduleEntities));
        }

        public async Task<Result<ScheduleDto>> GetScheduleByIdAsync(long id)
        {
            var scheduleEntity = await _unitOfWork.ScheduleRepository.GetByIdAsync(id);

            return scheduleEntity == null ?
                Result<ScheduleDto>.GetError(ErrorCode.NotFound, $"Schedule with id={id} does not exist") :
                Result<ScheduleDto>.GetSuccess(_mapper.Map<ScheduleDto>(scheduleEntity));
        }

        public async Task<Result<IList<ScheduleDto>>> GetSchedulesByStudentGroupIdAsync(long id)
        {
            var groupEntity = await _unitOfWork.StudentGroupRepository.GetByIdAsync(id);

            if (groupEntity == null)
            {
                return null;
            }
            var schedulesOfGroup = await _unitOfWork.ScheduleRepository.GetSchedulesByStudentGroupIdAsync(id);

            return Result<IList<ScheduleDto>>.GetSuccess(
                _mapper.Map<IList<ScheduleDto>>(schedulesOfGroup));
        }

        public async Task<Result<ScheduleDto>> UpdateStudentGroupAsync(long scheduleId, UpdateScheduleDto scheduleDTO)
        {
            try
            {

                if (scheduleDTO == null)
                {
                    return Result<ScheduleDto>.GetError(ErrorCode.NotFound, "UpdateScheduleDto is null");
                }
                var foundSchedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(scheduleId);

                if (foundSchedule == null)
                {
                    return Result<ScheduleDto>.GetError(ErrorCode.NotFound,
                        $"Schedule with id={scheduleId} does not exist");
                }
                var updatedEntity = _mapper.Map<Schedule>(scheduleDTO);

                var errorMessage = Validate(updatedEntity);

                if (errorMessage != null)
                {
                    Result<ScheduleDto>.GetError(ErrorCode.ValidationError, errorMessage);
                }

                foundSchedule.RepeatRate = updatedEntity.RepeatRate;

                if (updatedEntity.DayNumber != null)
                {
                    foundSchedule.DayNumber = updatedEntity.DayNumber;
                }
                await _unitOfWork.CommitAsync();

                return Result<ScheduleDto>.GetSuccess(_mapper.Map<ScheduleDto>(foundSchedule));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<ScheduleDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        private bool IsNotValidRepeating(Schedule entity)
        {
            return entity.RepeatRate != RepeatRate.Daily &&
                entity.RepeatRate != RepeatRate.Never &&
                      entity.DayNumber == null;
        }

        private string Validate(Schedule schedule)
        {

            if (IsNotValidRepeating(schedule))
            {
                return "DayNumber can`t be null because RepeatRate is not either 0 or 1";
            }

            if (schedule.LessonEnd <= schedule.LessonStart)
            {
                return "LessonEnd can`t be less than or equal to LessonStart";
            }

            return null;
        }

    }
}
