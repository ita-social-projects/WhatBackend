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
        public const int DaysInOneWeek = 7;
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

            int daysCount = (int)(finish.Date - start.Date).TotalDays;

            List<TagBuilder> dayContainers = new List<TagBuilder>();

            for (int day = 1; day <= daysCount; day++)
            {
                dayContainers.Add(GetDayContainerHtml(start.AddDays(day), calendar.ScheduledEvents));
            }

            if (finish.DayOfWeek != DayOfWeek.Saturday)
            {
                int daysToAppendCount = DaysInOneWeek - (int)finish.DayOfWeek;

                int daysRangeLength = daysCount + daysToAppendCount;

                for (int day = daysCount + 1; day < daysRangeLength; day++)
                {
                    dayContainers.Add(GetDateContainerHtml(start.AddDays(day)));
                }

                daysCount += daysToAppendCount;
            }

            if (start.AddDays(1).DayOfWeek != DayOfWeek.Sunday)
            {
                int daysToPrependCount = (int)start.DayOfWeek;

                for (int i = 0; i <= daysToPrependCount; i++)
                {
                    dayContainers.Insert(0, GetDateContainerHtml(start.AddDays(-i)));
                }
            }

            double rowCount = (double)daysCount / DaysInOneWeek;

            if (rowCount - (int)rowCount != 0)
                rowCount = (int)rowCount + 1;

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
            int daysRangeLength = row * DaysInOneWeek + DaysInOneWeek;
            TagBuilder rowBlock = new TagBuilder("div");
            rowBlock.AddCssClass("row");

            if (daysRangeLength > days.Count)
                daysRangeLength = days.Count;

            for (int i = row * DaysInOneWeek; i < daysRangeLength; i++)
            {
                rowBlock.InnerHtml.AppendHtml(days[i]);
            }

            return rowBlock;
        }

        public static TagBuilder GetDayContainerHtml(
            DateTime current,
            IList<CalendarScheduledEventViewModel> events)
        {
            var eventsFiltered = events.Where(x => x.EventFinish.Date >= current.Date && x.EventStart.Date <= current.Date).ToList();

            return GetDateContainerHtml(current, eventsFiltered);
        }

        public static TagBuilder GetStyledDateBlock(DateTime day)
        {
            string rowClass = day.DayOfWeek == DayOfWeek.Sunday ||
                day.DayOfWeek == DayOfWeek.Saturday ?
                "col-1" : "col-2";

            TagBuilder div = new TagBuilder("div");
            div.AddCssClass(rowClass);

            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("badge badge-info");
            span.InnerHtml.Append(day.Day.ToString());

            div.InnerHtml.AppendHtml(span);

            return div;
        }

        public static TagBuilder GetDateContainerHtml(DateTime day)
        {
            TagBuilder div = GetStyledDateBlock(day);

            div.AddCssClass("out-of-range");

            return div;
        }

        public static TagBuilder GetDateContainerHtml(DateTime day,
            IEnumerable<CalendarScheduledEventViewModel> events)
        {
            TagBuilder div = GetStyledDateBlock(day);

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

            foreach (var e in events)
            {
                TagBuilder button = GetButtonContainerHtml(e, btnClass);

                divEvents.InnerHtml.AppendHtml(button);
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
