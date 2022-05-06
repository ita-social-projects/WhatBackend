using CharlieBackend.Core.DTO.Event;
using CharlieBackend.Core.DTO.Schedule;
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
                error.Append(" Group does not exist");
            }

            if (request.Context.MentorID.HasValue && !await _unitOfWork.MentorRepository.IsEntityExistAsync(request.Context.MentorID.Value))
            {
                error.Append(" Mentor does not exist");
            }

            if ((await _unitOfWork.AccountRepository.GetAccountCredentialsById(request.Context.MentorID.Value)).IsActive == false)
            {
                error.Append(" Mentor is not active");
            }

            if (request.Context.ThemeID.HasValue && !await _unitOfWork.ThemeRepository.IsEntityExistAsync(request.Context.ThemeID.Value))
            {
                error.Append(" Theme does not exist");
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
                error.Append(" Course does not exist");
            }

            if (request.GroupID.HasValue && !await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(request.GroupID.Value))
            {
                error.Append(" Group does not exist");
            }

            if (request.MentorID.HasValue && !await _unitOfWork.MentorRepository.IsEntityExistAsync(request.MentorID.Value))
            {
                error.Append(" Mentor does not exist");
            }

            if (request.StudentAccountID.HasValue && !await _unitOfWork.StudentRepository.IsEntityExistAsync(request.StudentAccountID.Value))
            {
                error.Append(" Student does not exist");
            }

            if (request.ThemeID.HasValue && !await _unitOfWork.ThemeRepository.IsEntityExistAsync(request.ThemeID.Value))
            {
                error.Append(" Theme does not exist");
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
            StringBuilder error = new StringBuilder(string.Empty);

            if (!await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(request.StudentGroupId))
            {
                error.Append(" Group does not exist");
            }

            if (request.MentorId.HasValue && !await _unitOfWork.MentorRepository.IsEntityExistAsync(request.MentorId.Value))
            {
                error.Append(" Mentor does not exist");
            }

            if (request.ThemeId.HasValue && !await _unitOfWork.ThemeRepository.IsEntityExistAsync(request.ThemeId.Value))
            {
                error.Append(" Theme does not exist");
            }

            return error.Length > 0 ? error.ToString() : null;
        }
    }
}