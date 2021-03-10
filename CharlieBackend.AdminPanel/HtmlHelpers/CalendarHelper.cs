using CharlieBackend.AdminPanel.Models.Calendar;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace CharlieBackend.AdminPanel.HtmlHelpers
{
    public static class CalendarHelper
    {

        public static HtmlString CalendarBodyHtml(this IHtmlHelper html, CalendarViewModel calendar)
        {
            DateTime start, finish;

            var eventOccurencesFiltered = calendar.ScheduledEvents.Select(x => calendar.EventOccurences.First(y => y.Id == x.EventOccuranceId)).ToList();

            if (calendar.ScheduledEventFilter.FinishDate.HasValue)
            {
                finish = calendar.ScheduledEventFilter.FinishDate.Value;
            }
            else
            {
                DateTime latestFinish = eventOccurencesFiltered.Max(x => x.EventFinish);
                DateTime latestStart = eventOccurencesFiltered.Max(x => x.EventStart);

                finish = latestFinish > latestStart ? latestFinish : latestStart;
            }

            if (calendar.ScheduledEventFilter.StartDate.HasValue)
            {
                start = calendar.ScheduledEventFilter.StartDate.Value;
            }
            else
            {
                DateTime earliestFinish = eventOccurencesFiltered.Min(x => x.EventFinish);
                DateTime earliestStart = eventOccurencesFiltered.Min(x => x.EventStart);

                start = earliestFinish > earliestStart ? earliestStart : earliestFinish;
            }

            double daysCount = (finish - start).TotalDays;
            double rowCount = daysCount / 7;

            if (rowCount - (int)rowCount != 0)
                rowCount = (int)rowCount + 1;

            if (finish.DayOfWeek != DayOfWeek.Saturday)
                daysCount += 7 - (int)finish.DayOfWeek;

            List<TagBuilder> dayContainers = new List<TagBuilder>();

            for (int day = 0; day < daysCount; day++)
            {
                dayContainers.Add(GetDayContainerHtml(start.AddDays(day), calendar.ScheduledEvents));
            }

            int startDay = (int)start.DayOfWeek;

            if (start.DayOfWeek != DayOfWeek.Sunday)
            {
                for (int i = 1; i <= startDay; i++)
                {
                    dayContainers = dayContainers.Prepend(GetDateContainerHtml(start.AddDays(-i))).ToList();
                }
            }

            List<TagBuilder> rowContainers = new List<TagBuilder>();

            for (int i = 0; i < rowCount; i++)
            {
                rowContainers.Add(GetRowHtml(i, dayContainers));
            }

            string result = string.Empty;

            foreach (var row in rowContainers)
            {
                using (var writer = new System.IO.StringWriter())
                {
                    row.WriteTo(writer, HtmlEncoder.Default);

                    result += writer.ToString();
                }
            }

            return new HtmlString(result);
        }

        public static TagBuilder GetRowHtml(int row, List<TagBuilder> days)
        {
            int len = row * 7 + 7;
            TagBuilder rowBlock = new TagBuilder("div");
            rowBlock.AddCssClass("row");

            try
            {
                for (int i = row * 7; i < len; i++)
                {
                    rowBlock.InnerHtml.AppendHtml(days[i]);
                }
            }
            catch
            {

            }

            return rowBlock;
        }

        public static TagBuilder GetDayContainerHtml(
            DateTime current,
            IList<CalendarScheduledEventViewModel> events = null)
        {
            if (events == null)
                return GetDateContainerHtml(current);

            try
            {
                var eventsFiltered = events.Where(x => x.EventFinish.Date >= current.Date && x.EventStart.Date <= current.Date).ToList();

                return GetDateContainerHtml(current, eventsFiltered);
            }
            catch
            {
                return GetDateContainerHtml(current);
            }
        }

        public static TagBuilder GetDateContainerHtml(DateTime day, IEnumerable<CalendarScheduledEventViewModel> events = null)
        {
            string btnClass = string.Empty;
            string rowClass = day.DayOfWeek == DayOfWeek.Sunday ||
                day.DayOfWeek == DayOfWeek.Saturday ?
                "col-1" : "col-2";

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

            TagBuilder div = new TagBuilder("div");
            div.AddCssClass(rowClass);

            TagBuilder divEvents = new TagBuilder("div");
            divEvents.AddCssClass("events");

            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("badge badge-info");
            span.InnerHtml.Append(day.Day.ToString());

            div.InnerHtml.AppendHtml(span);

            try
            {
                foreach (var e in events)
                {
                    TagBuilder button = GetButtonContainerHtml(e, btnClass);
                   
                    divEvents.InnerHtml.AppendHtml(button);
                }
            }
            catch
            {

            }

            div.InnerHtml.AppendHtml(divEvents);

            return div;
        }

        public static TagBuilder GetButtonContainerHtml(CalendarScheduledEventViewModel model, string cssClass)
        {
            TagBuilder button = new TagBuilder("button");

            button.AddCssClass(cssClass);
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-toggle", "modal");
            button.Attributes.Add("data-target", "#seeSchedulEvent");
            button.Attributes.Add("seGroupdID", model.StudentGroupId.ToString());
            button.Attributes.Add("seMentorId", model.MentorId.ToString());
            button.Attributes.Add("seLessonId", model.LessonId.ToString());
            button.Attributes.Add("seThemeId", model.ThemeId.ToString());
            button.InnerHtml.Append(model.Id.ToString());

            return button;
        }
    }
}
