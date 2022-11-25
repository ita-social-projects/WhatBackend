using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Event;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers
{
    /// <summary>
    /// Class that make requests to database to verify context IDs
    /// </summary>
    public class SchedulesEventsDbEntityVerifier : ISchedulesEventsDbEntityVerifier
    {
        private readonly IUnitOfWork _unitOfWork;

        public SchedulesEventsDbEntityVerifier(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> ValidateCreateScheduleRequestAsync(CreateScheduleDto request)
        {
            StringBuilder error = new StringBuilder(string.Empty);

            if (!await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(request.Context.GroupID))
            {
                error.Append(ResponseMessages.NotExist("Group"));
            }

            if (request.Context.MentorID.HasValue && await _unitOfWork.MentorRepository.IsEntityExistAsync(request.Context.MentorID.Value))
            {
                if ((await _unitOfWork.AccountRepository.GetAccountCredentialsById(request.Context.MentorID.Value)).IsActive == false)
                {
                    error.Append(ResponseMessages.NotActive("Mentor"));
                }
            }
            else
            {
                error.Append(ResponseMessages.NotExist("Mentor"));
            }

            if (request.Context.ThemeID.HasValue && !await _unitOfWork.ThemeRepository.IsEntityExistAsync(request.Context.ThemeID.Value))
            {
                error.Append(ResponseMessages.NotExist("Theme"));
            }

            if (request.Pattern.Index.HasValue && (request.Pattern.Index.Value > MonthIndex.Last || request.Pattern.Index.Value < MonthIndex.Undefined))
            {
                error.Append(Resources.SharedResources.IndexNotValidResponseMessage);
            }

            if (request.Pattern.Type > PatternType.RelativeMonthly || request.Pattern.Type < PatternType.Daily)
            {
                error.Append(ResponseMessages.NotValid(nameof(PatternType)));
            }

            if (request.Range.StartDate < System.DateTime.Now)
            {
                error.Append(ResponseMessages.NotValid("StartDate"));
            }

            return error.Length > 0 ? error.ToString() : null;
        }

        public async Task<string> ValidateEventOccuranceId(long id)
        {
            string result = null;

            if (!await _unitOfWork.EventOccurrenceRepository.IsEntityExistAsync(id))
            {
                result = $"EventOccurance id={id} does not exist";
            }

            return result;
        }

        public async Task<string> ValidateGetEventsFilteredRequest(ScheduledEventFilterRequestDTO request)
        {
            StringBuilder error = new StringBuilder(string.Empty);

            if (request.CourseID.HasValue && !await _unitOfWork.CourseRepository.IsEntityExistAsync(request.CourseID.Value))
            {
                error.Append(ResponseMessages.NotExist("Course"));
            }

            if (request.GroupID.HasValue && !await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(request.GroupID.Value))
            {
                error.Append(ResponseMessages.NotExist("Group"));
            }

            if (request.MentorID.HasValue && !await _unitOfWork.MentorRepository.IsEntityExistAsync(request.MentorID.Value))
            {
                error.Append(ResponseMessages.NotExist("Mentor"));
            }

            if (request.StudentAccountID.HasValue && !await _unitOfWork.StudentRepository.IsEntityExistAsync(request.StudentAccountID.Value))
            {
                error.Append(ResponseMessages.NotExist("Student"));
            }

            if (request.ThemeID.HasValue && !await _unitOfWork.ThemeRepository.IsEntityExistAsync(request.ThemeID.Value))
            {
                error.Append(ResponseMessages.NotExist("Theme"));
            }

            return error.Length > 0 ? error.ToString() : null;
        }

        public async Task<string> ValidateScheduledEventId(long id)
        {
            string result = null;

            if (!await _unitOfWork.ScheduledEventRepository.IsEntityExistAsync(id))
            {
                result = $"ScheduledEvent id={id} does not exist";
            }

            return result;
        }

        public async Task<string> ValidateCreateScheduleEventRequestAsync(CreateSingleEventDto request)
        {
            StringBuilder error = new StringBuilder();

            if (!await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(request.StudentGroupId))
            {
                error.Append(ResponseMessages.NotExist("Group"));
            }

            if (request.MentorId.HasValue && !await _unitOfWork.MentorRepository.IsEntityExistAsync(request.MentorId.Value))
            {
                error.Append(ResponseMessages.NotExist("Mentor"));
            }

            if (request.ThemeId.HasValue && !await _unitOfWork.ThemeRepository.IsEntityExistAsync(request.ThemeId.Value))
            {
                error.Append(ResponseMessages.NotExist("Theme"));
            }

            return error.Length > 0 ? error.ToString() : null;
        }

        public async Task<string> ValidateUpdatedScheduleAsync(UpdateScheduledEventDto updatedSchedule)
        {
            StringBuilder error = new StringBuilder();

            if (await _unitOfWork.MentorRepository.GetByIdAsync(updatedSchedule.MentorId.GetValueOrDefault()) is null)
            {
                error.Append(ResponseMessages.NotExist("Mentor"));
            }
            if (await _unitOfWork.ThemeRepository.GetByIdAsync(updatedSchedule.ThemeId.GetValueOrDefault()) is null)
            {
                error.Append(ResponseMessages.NotExist("Theme"));
            }
            if (await _unitOfWork.StudentGroupRepository.GetByIdAsync(updatedSchedule.StudentGroupId.GetValueOrDefault()) is null)
            {
                error.Append(ResponseMessages.NotExist("Group"));
            }

            return error.Length > 0 ? error.ToString() : null;
        }

        public async Task<string> ValidateConnectEventToLessonAsync (long eventId, long lessonId)
        {
            StringBuilder error = new StringBuilder();

            if (await _unitOfWork.ScheduledEventRepository.IsLessonConnectedToSheduledEventAsync(lessonId))
            {
                error.Append($"Lesson with id={lessonId} is already associated with another ScheduledEvent. ");
            }
            if(!await _unitOfWork.ScheduledEventRepository.IsEntityExistAsync(eventId))
            {
                error.Append(ResponseMessages.NotExist("EventID"));
            }
            if (!await _unitOfWork.LessonRepository.IsEntityExistAsync(lessonId))
            {
                error.Append(ResponseMessages.NotExist("LessonID"));
            }

            return error.Length > 0 ? error.ToString() : null;
        }
    }
}