using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class EventsService : IEventsService
    {
        private readonly EventsApiEndpoints _eventsApiEndpoints;
        private readonly IApiUtil _apiUtil;

        public EventsService(IOptions<ApplicationSettings> options,
            IApiUtil apiUtil)
        {
            _eventsApiEndpoints = options.Value.Urls.ApiEndpoints.Events;
            _apiUtil = apiUtil;
        }

        public async Task ConnectScheduleToLessonById(long id, LessonDto lesson)
        {
            var eventToLesson = string.Format(_eventsApiEndpoints.ConnectEventToLesson, id);
            await _apiUtil.PutAsync(eventToLesson, lesson);
        }
    }
}
