using CharlieBackend.Panel.Models.Calendar;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using CharlieBackend.Core.Extensions;
using System.Text;
using System.IO;

namespace CharlieBackend.Panel.HtmlHelpers
{
    public static class CalendarHelper
    {
        #region constants

        private const string WorkingDayOfFullWeekClass = "col-calendar-full-week";
        private const string WeekendDayOfFullWeekClass = "col-calendar-full-week weekend";
        private const string WorkingDayOfWorkingWeekClass = "col-calendar-working-week";
        private const string WeekendDayOfWorkingWeekClass = "d-none weekend";

        #endregion
        public static HtmlString CalendarHeaderHtml(this IHtmlHelper html, CalendarDisplayType displayType)
        {
            StringBuilder result = new StringBuilder();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                TagBuilder div = GetDayBlock(day, displayType);

                div.InnerHtml.Append(day.ToString());

                using var writer = new System.IO.StringWriter();
                div.WriteTo(writer, HtmlEncoder.Default);
                result.Append(writer.ToString());
            }

            return new HtmlString(result.ToString());
        }

        public static HtmlString CalendarBodyHtml(this IHtmlHelper html, CalendarViewModel calendar)
        {
            var eventOccurencesFiltered = calendar.ScheduledEvents.Select(x => calendar.EventOccurences.FirstOrDefault(y => y.Id == x.EventOccuranceId)).ToList();

            DateTime startDate = calendar.ScheduledEventFilter.StartDate ?? GetStartDate(eventOccurencesFiltered);
            DateTime finishDate = calendar.ScheduledEventFilter.FinishDate ?? GetFinishDate(eventOccurencesFiltered);

            StringBuilder result = new StringBuilder();

            foreach (var item in GetRowContainers(GetScheduledEventModels(calendar, startDate, finishDate), startDate, finishDate, calendar.DisplayType))
            {
                using var writer = new System.IO.StringWriter();
                item.WriteTo(writer, HtmlEncoder.Default);
                result.Append(writer.ToString());
            }

            return new HtmlString(result.ToString());
        }

        private static IList<CalendarScheduledEventModel> GetScheduledEventModels(CalendarViewModel calendar, DateTime startDate, DateTime finishDate)
        {
            var models = calendar.ScheduledEvents.Where(i => i.EventStart >= startDate && i.EventFinish <= finishDate)
                .Select(scheduledEvent => new CalendarScheduledEventModel
                {
                    Theme = calendar.Themes.First(t => t.Id == scheduledEvent.ThemeId).Name,
                    MentorFirstName = calendar.Mentors.First(m => m.Id == scheduledEvent.MentorId).FirstName,
                    MentorLastName = calendar.Mentors.First(m => m.Id == scheduledEvent.MentorId).LastName,
                    StudentGroup = calendar.StudentGroups.First(s => s.Id == scheduledEvent.StudentGroupId).Name,
                    EventStart = scheduledEvent.EventStart,
                    EventFinish = scheduledEvent.EventFinish,
                    EventOccurrenceId = scheduledEvent.EventOccuranceId,
                    SingleEventId = scheduledEvent.Id,
                    Color = calendar.EventOccurences.FirstOrDefault(x => x.Id.Equals(scheduledEvent?.EventOccuranceId)) != null ?
                    calendar.EventOccurences.FirstOrDefault(x => x.Id.Equals(scheduledEvent?.EventOccuranceId)).Color : scheduledEvent.Color
                }).OrderBy(i => i.EventStart)
                .ToList();

            return models;
        }

        private static IList<TagBuilder> GetRowContainers(IList<CalendarScheduledEventModel> models, DateTime startDate, DateTime finishDate, CalendarDisplayType displayType)
        {
            IList<IList<TagBuilder>> daysContainers = new List<IList<TagBuilder>>();
            var list = GetDaysContainer(models, startDate, finishDate, displayType);

            while(list.Count != 0)
            {
                daysContainers.Add(list.Take(DateTimeExtensions.DayInWeekCount).ToList());
                list = list.Skip(DateTimeExtensions.DayInWeekCount).ToList();
            }

            return GetRowContainers(daysContainers);
        }

        private static IList<TagBuilder> GetRowContainers(IList<IList<TagBuilder>> daysContainers)
        {
            IList<TagBuilder> rowsBlock = new List<TagBuilder>();

            foreach (var daysContainer in daysContainers)
            {
                TagBuilder rowBlock = new TagBuilder("div");
                rowBlock.AddCssClass("row");
                foreach (var tag in daysContainer)
                {
                    rowBlock.InnerHtml.AppendHtml(tag);
                }
                rowsBlock.Add(rowBlock);
            }

            return rowsBlock;
        }

        private static IList<TagBuilder> GetDaysContainer(IList<CalendarScheduledEventModel> models, DateTime startDate, DateTime finishDate, CalendarDisplayType displayType)
        {
            IList<TagBuilder> dayContainers = new List<TagBuilder>();

            var startOfWeek = startDate.StartOfWeek(DayOfWeek.Sunday);
            for (DateTime date = startOfWeek; date.Date < startDate.Date; date = date.AddDays(1))
            {
                dayContainers.Add(GetDayBlockWithDateWithOutOfRange(date, displayType));
            }

            for (DateTime date = startDate; date.Date < finishDate.Date; date = date.AddDays(1))
            {
                dayContainers.Add(GetDayBlockWithEvents(models.Where(x => x.EventFinish.Date >= date.Date && x.EventStart.Date <= date.Date), date, displayType));
            }

            var endOfWeek = finishDate.EndOfWeek(DayOfWeek.Saturday);
            for (DateTime date = finishDate; date.Date <= endOfWeek.Date; date = date.AddDays(1))
            {
                dayContainers.Add(GetDayBlockWithDateWithOutOfRange(date, displayType));
            }

            return dayContainers;
        }

        private static TagBuilder GetDayBlockWithEvents(IEnumerable<CalendarScheduledEventModel> models, DateTime day, CalendarDisplayType displayType)
        {
            TagBuilder div = GetDayBlockWithDate(day, displayType);

            TagBuilder divEvents = new TagBuilder("div");
            divEvents.AddCssClass("events");
            var cssClass = GetButtonCssClass(day);
             foreach (var e in models)
            {
                TagBuilder button = GetButtonContainerHtml(e, cssClass);

                divEvents.InnerHtml.AppendHtml(button);
            }

            div.InnerHtml.AppendHtml(divEvents);

            return div;
        }

        private static string GetButtonCssClass(DateTime day)
        {
            string btnClass = string.Empty;

            if (day.Date == DateTime.Now.Date)
            {
                btnClass = "btn btn-outline-success btn-event";
            }
            if (day.Date > DateTime.Now.Date)
            {
                btnClass = "btn btn-outline-primary btn-event";
            }
            if (day.Date < DateTime.Now.Date)
            {
                btnClass = "btn btn-outline-dark btn-event";
            }
            
            return btnClass;
        }

        private static TagBuilder GetDayBlockWithDateWithOutOfRange(DateTime day, CalendarDisplayType displayType)
        {
            TagBuilder dayBlock = GetDayBlockWithDate(day, displayType);

            dayBlock.AddCssClass("out-of-range");

            return dayBlock;
        }

        private static TagBuilder GetDayBlockWithDate(DateTime date, CalendarDisplayType displayType)
        {
            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("badge badge-info");
            span.InnerHtml.Append(date.Day.ToString());

            var dayBlock = GetDayBlock(date, displayType);
            dayBlock.InnerHtml.AppendHtml(span);

            return dayBlock;
        }

        private static TagBuilder GetDayBlock(DayOfWeek dayOfWeek, CalendarDisplayType displayType)
        {
            TagBuilder div = new TagBuilder("div");

            string divClass = GetDayOfWeekClass(dayOfWeek, displayType);

            div.AddCssClass(divClass);

            return div;
        }

        private static TagBuilder GetDayBlock(DateTime date, CalendarDisplayType displayType)
        {
            TagBuilder div = new TagBuilder("div");

            string rowClass = GetDayOfWeekClass(date.DayOfWeek, displayType);

            div.AddCssClass(rowClass);

            return div;
        }

        private static TagBuilder GetButtonContainerHtml(CalendarScheduledEventModel model, string cssClass)
        {
            TagBuilder button = new TagBuilder("button");
            button.AddCssClass(cssClass);
            button.AddCssClass("auto-scroll");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-toggle", "modal");
            button.Attributes.Add("data-target", "#seeSchedulEvent");
            button.Attributes.Add("seTime", $"{ model.EventStart:HH:mm} - {model.EventFinish:HH:mm}");
            button.Attributes.Add("seGroup", model.StudentGroup);
            button.Attributes.Add("seMentor", $"{model.MentorFirstName} {model.MentorLastName}");
            button.Attributes.Add("seTheme", model.Theme);
            button.Attributes.Add("seEventOccurrenceId", $"{model.EventOccurrenceId}");
            button.Attributes.Add("seSingleEventId", $"{model.SingleEventId}");
            button.Attributes.Add($"style = \"border-color: #{ model.Color.ToString("X")}; color: #{model.Color.ToString("X")} \"", "");
            button.InnerHtml.Append($"{model.EventStart:HH:mm} - {model.EventFinish:HH:mm} \n");
            button.InnerHtml.Append($"{model.Theme}; ");
            button.InnerHtml.Append($"{model.StudentGroup}; ");
            button.InnerHtml.Append($"{model.MentorFirstName} {model.MentorLastName}");

            return button;
        }

        private static string GetDayOfWeekClass(DayOfWeek dayOfWeek, CalendarDisplayType displayType)
        {
            string dayOfWeekClass = string.Empty;

            switch (displayType)
            {
                case CalendarDisplayType.FullWeek:
                    dayOfWeekClass = GetDayOFullWeekClass(dayOfWeek);
                    break;

                case CalendarDisplayType.WorkingWeek:
                    dayOfWeekClass = GetDayOfWorkingWeekClass(dayOfWeek);
                    break;

                default:
                    dayOfWeekClass = GetDayOfWorkingWeekClass(dayOfWeek);
                    break;
            }

            return dayOfWeekClass;
        }

        private static string GetDayOFullWeekClass(DayOfWeek dayOfWeek)
        {
            return IsWeekend(dayOfWeek) ? WeekendDayOfFullWeekClass : WorkingDayOfFullWeekClass;
        }

        private static string GetDayOfWorkingWeekClass(DayOfWeek dayOfWeek)
        {
            return IsWeekend(dayOfWeek) ? WeekendDayOfWorkingWeekClass : WorkingDayOfWorkingWeekClass;
        }

        private static DateTime GetFinishDate(IList<CalendarEventOccurrenceViewModel> models)
        {
            DateTime latestFinish = models.Max(x => x.EventFinish);
            DateTime latestStart = models.Max(x => x.EventStart);

            return latestFinish > latestStart ? latestFinish : latestStart;
        }

        private static DateTime GetStartDate(IList<CalendarEventOccurrenceViewModel> models)
        {
            DateTime earliestFinish = models.Min(x => x.EventFinish);
            DateTime earliestStart = models.Min(x => x.EventStart);

            return earliestFinish > earliestStart ? earliestStart : earliestFinish;
        }

        private static bool IsWeekend(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }
    }
}
