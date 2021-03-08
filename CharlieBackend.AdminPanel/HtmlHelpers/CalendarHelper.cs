using CharlieBackend.AdminPanel.Models.Calendar;
using CharlieBackend.AdminPanel.Models.EventOccurence;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static CharlieBackend.Business.Services.ScheduleServiceFolder.EventOccuranceStorageParser;

namespace CharlieBackend.AdminPanel.HtmlHelpers
{
    public static class CalendarHelper
    {
        public static TagBuilder CreateTagWithValueAndText(string tagName, string value, string text)
        {
            TagBuilder tag = new TagBuilder(tagName);
            tag.Attributes.Add("value", value);
            tag.InnerHtml.Append(text);

            return tag;
        }

        public static HtmlString GroupSelect(this IHtmlHelper html, CalendarViewModel calendar)
        {
            TagBuilder select = new TagBuilder("select");
            select.Attributes.Add("class", "custom-select");

            if (calendar.ScheduledEventFilter.GroupID.HasValue)
                foreach (var group in calendar.StudentGroups)
                {

                    var tag = CreateTagWithValueAndText("option", group.Id.ToString(), group.FinishDate.ToString());

                    select.InnerHtml.AppendHtml(tag);
                }

            var writer = new System.IO.StringWriter();
            select.WriteTo(writer, HtmlEncoder.Default);

            return new HtmlString(writer.ToString());
        }

        public static HtmlString CalendarBodyHtml(this IHtmlHelper html, CalendarViewModel calendar)
        {
            DateTime start, finish;

            var eventOccurencesFiltered = calendar.ScheduledEvents.Select(x => calendar.EventOccurences.First(y => y.Id == x.Id));

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

            List<TagBuilder> dayContainers = new List<TagBuilder>();

            for (int day = 0; day < daysCount; day++)
            {
                dayContainers.Add(GetDayContainerHtml(day, start, calendar.ScheduledEvents, eventOccurencesFiltered));
            }

            int startDay = (int)start.DayOfWeek;

            for (int i = 1; i <+ startDay; i++)
            {
                dayContainers = dayContainers.Prepend(GetDayContainerHtml(-i, start)).ToList();
            }

            List<TagBuilder> rowContainers = new List<TagBuilder>();

            for (int i = 0; i < rowCount; i++)
            {
                rowContainers.Add(GetRowHtml(i, dayContainers));
            }

            string result = string.Empty;

            foreach(var row in rowContainers)
            {
                using (var writer = new System.IO.StringWriter())
                {
                    row.WriteTo(writer, HtmlEncoder.Default);

                    result += "<hr>" + writer.ToString();
                }
            }

            return new HtmlString(result);
        }

        public static TagBuilder GetRowHtml(int row, List<TagBuilder> days)
        {
            int len = row*7 + 7;

            TagBuilder rowBlock = new TagBuilder("div");
            rowBlock.AddCssClass("row");
            for(int i=row*7; i<len;i++)
            {
                rowBlock.InnerHtml.AppendHtml(days[i]);
            }

            return rowBlock;
        }

        public static TagBuilder GetDayContainerHtml(int day, DateTime start)
        {
            start = start.AddDays(day);

            int monthDay = start.Day;

            return GetDateContainerHtml(start, null);
        }

        public static TagBuilder GetDayContainerHtml(int day, DateTime start, IEnumerable<ScheduledEventDTO> events, IEnumerable<EventOccurenceViewModel> occurences)
        {
            start = start.AddDays(day);

            int monthDay = start.Day;

            var eventsFiltered = events.Where(x => {

                var storage = GetFullDataFromStorage(occurences.First(y=>y.Id==x.EventOccuranceId).Storage);

                return storage.Dates.Contains(day);

            });

            return GetDateContainerHtml(start, eventsFiltered);
        }

        public static TagBuilder GetDateContainerHtml(DateTime day, IEnumerable<ScheduledEventDTO> events)
        {
            string btnClass = string.Empty;
            string rowClass = day.DayOfWeek == DayOfWeek.Sunday ||
                day.DayOfWeek == DayOfWeek.Saturday ?
                "col-1" : "col-2";

            if (day == DateTime.Now)
            {
                btnClass = "btn btn-outline-success";
            }
            if (day > DateTime.Now)
            {
                btnClass = "btn btn-outline-primary";
            }
            if (day < DateTime.Now)
            {
                btnClass = "btn btn-outline-dark";
            }

            TagBuilder div = new TagBuilder("div");
            div.AddCssClass(rowClass);

            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("badge badge-info");
            span.InnerHtml.Append(day.Day.ToString());

            div.InnerHtml.AppendHtml(span);

            if(events!=null)
            foreach (var e in events)
            {
                TagBuilder button = new TagBuilder("button");
                button.AddCssClass(btnClass);
                button.Attributes.Add("type", "button");
                button.InnerHtml.Append(e.Id.ToString());
                div.InnerHtml.AppendHtml(button);
            }

            return div;
        }
    }
}
