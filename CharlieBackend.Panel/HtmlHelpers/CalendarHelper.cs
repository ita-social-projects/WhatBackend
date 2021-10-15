using CharlieBackend.Panel.Models.Calendar;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using CharlieBackend.Core.Extensions;

namespace CharlieBackend.Panel.HtmlHelpers
{
    public static class CalendarHelper
    {
        public static HtmlString CalendarBodyHtml(this IHtmlHelper html, CalendarViewModel calendar)
        {
            var eventOccurencesFiltered = calendar.ScheduledEvents.Select(x => calendar.EventOccurences.First(y => y.Id == x.EventOccuranceId)).ToList();

            DateTime startDate = calendar.ScheduledEventFilter.StartDate ?? GetStartData(eventOccurencesFiltered);
            DateTime finishDate = calendar.ScheduledEventFilter.FinishDate ?? GetFinishData(eventOccurencesFiltered);

            string result = string.Empty;
            foreach (var item in GetRowContainers(GetScheduledEventModels(calendar, startDate, finishDate), startDate, finishDate))
            {
                using var writer = new System.IO.StringWriter();
                item.WriteTo(writer, HtmlEncoder.Default);
                result += writer.ToString();
            }

            return new HtmlString(result);
        }

        private static IList<CalendarScheduledEventModel> GetScheduledEventModels(CalendarViewModel calendar, DateTime startDate, DateTime finishDate)
        {
            IList<CalendarScheduledEventModel> models = new List<CalendarScheduledEventModel>();

            var scheduledEvents = calendar.ScheduledEvents.Where(i => i.EventStart >= startDate && i.EventFinish <= finishDate);

            foreach (var item in scheduledEvents)
            {
                var theme = calendar.Themes.First(t => t.Id == item.ThemeId).Name;
                var mentorFirstName = calendar.Mentors.First(m => m.Id == item. MentorId).FirstName;
                var mentorLastName = calendar.Mentors.First(m => m.Id == item. MentorId).LastName;
                var studentGroup = calendar.StudentGroups.First(s => s.Id == item.StudentGroupId).Name;
                var eventStart = item.EventStart;
                var eventFinish = item.EventFinish;

                var model = new CalendarScheduledEventModel
                {
                    Theme = theme,
                    MentorFirstName = mentorFirstName,
                    MentorLastName = mentorLastName,
                    StudentGroup = studentGroup,
                    EventStart = eventStart,
                    EventFinish = eventFinish
                };
                models.Add(model);
            }

            return models;
        }

             
        private static IList<TagBuilder> GetRowContainers(IList<CalendarScheduledEventModel> models, DateTime startDate, DateTime finishDate)
        {
            IList<IList<TagBuilder>> daysContainers = new List<IList<TagBuilder>>();
            var list = GetDaysContainer(models, startDate, finishDate);

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

        private static IList<TagBuilder> GetDaysContainer(IList<CalendarScheduledEventModel> models, DateTime startDate, DateTime finishDate)
        {
            IList<TagBuilder> dayContainers = new List<TagBuilder>();

            var startOfWeek = startDate.StartOfWeek(DayOfWeek.Sunday);
            for (DateTime date = startOfWeek; date.Date < startDate.Date; date = date.AddDays(1))
            {
                dayContainers.Add(GetDayBlockWithDateWithOutOfRange(date));
            }

            for (DateTime date = startDate; date.Date < finishDate.Date; date = date.AddDays(1))
            {
                dayContainers.Add(GetDayBlockWithEvents(models.Where(x => x.EventFinish.Date >= date.Date && x.EventStart.Date <= date.Date), date));
            }

            var endOfWeek = finishDate.EndOfWeek(DayOfWeek.Saturday);
            for (DateTime date = finishDate; date.Date <= endOfWeek.Date; date = date.AddDays(1))
            {
                dayContainers.Add(GetDayBlockWithDateWithOutOfRange(date));
            }

            return dayContainers;
        }

        private static TagBuilder GetDayBlockWithEvents(IEnumerable<CalendarScheduledEventModel> models, DateTime day)
        {
            TagBuilder div = GetDayBlockWithDate(day);

            TagBuilder divEvents = new TagBuilder("div");
            divEvents.AddCssClass("events");

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

            foreach (var e in models)
            {
                TagBuilder button = GetButtonContainerHtml(e, btnClass);

                divEvents.InnerHtml.AppendHtml(button);
            }

            div.InnerHtml.AppendHtml(divEvents);

            return div;
        }

        private static TagBuilder GetDayBlockWithDateWithOutOfRange(DateTime day)
        {
            TagBuilder dayBlock = GetDayBlockWithDate(day);

            dayBlock.AddCssClass("out-of-range");

            return dayBlock;
        }

        private static TagBuilder GetDayBlockWithDate(DateTime date)
        {
            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("badge badge-info");
            span.InnerHtml.Append(date.Day.ToString());

            var dayBlock = GetDayBlock(date);
            dayBlock.InnerHtml.AppendHtml(span);

            return dayBlock;
        }

        private static TagBuilder GetDayBlock(DateTime date)
        {
            TagBuilder div = new TagBuilder("div");
            string rowClass = IsWeekend(date) ? "col-1" : "col-2";
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
            button.Attributes.Add("seGroup", model.StudentGroup);
            button.Attributes.Add("seMentor", $"{model.MentorFirstName} {model.MentorLastName}");
            button.Attributes.Add("seTheme", model.Theme);
            button.InnerHtml.Append($"{model.Theme}; ");
            button.InnerHtml.Append($"{model.StudentGroup}; ");
            button.InnerHtml.Append($"{model.MentorFirstName} ");
            button.InnerHtml.Append($"{model.MentorLastName}; ");

            return button;
        }

        private static DateTime GetFinishData(IList<CalendarEventOccurrenceViewModel> models)
        {
            DateTime latestFinish = models.Max(x => x.EventFinish);
            DateTime latestStart = models.Max(x => x.EventStart);

            return latestFinish > latestStart ? latestFinish : latestStart;
        }

        private static DateTime GetStartData(IList<CalendarEventOccurrenceViewModel> models)
        {
            DateTime earliestFinish = models.Min(x => x.EventFinish);
            DateTime earliestStart = models.Min(x => x.EventStart);

            return earliestFinish > earliestStart ? earliestStart : earliestFinish;
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }

    class CalendarScheduledEventModel
    {
        public string Theme { get; set; }

        public string MentorFirstName { get; set; }
        
        public string MentorLastName { get; set; }

        public string StudentGroup { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }
    }
}
