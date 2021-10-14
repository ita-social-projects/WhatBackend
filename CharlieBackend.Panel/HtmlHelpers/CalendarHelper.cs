using CharlieBackend.Panel.Models.Calendar;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace CharlieBackend.Panel.HtmlHelpers
{
    public static class CalendarHelper
    {
        public const int DaysInOneWeek = 15;
        public static HtmlString CalendarBodyHtml(this IHtmlHelper html, CalendarViewModel calendar)
        {
            var eventOccurencesFiltered = calendar.ScheduledEvents.Select(x => calendar.EventOccurences.First(y => y.Id == x.EventOccuranceId)).ToList();

            DateTime startDate = calendar.ScheduledEventFilter.StartDate ?? GetStartData(eventOccurencesFiltered);
            DateTime finishDate = calendar.ScheduledEventFilter.FinishDate ?? GetFinishData(eventOccurencesFiltered);

            //int daysCount = (int)(finishDate.Date - startDate.Date).TotalDays;
            //IList<TagBuilder> dayContainers = GetDaysContainersList(ref daysCount, calendar.ScheduledEvents, startDate, finishDate);
            //IList<TagBuilder> rowContainers = GetRowContainersList(daysCount, dayContainers);

            IList<TagBuilder> rowContainers = GetDaysContainer(startDate, finishDate);

            string result = string.Empty;
            using (var writer = new System.IO.StringWriter())
            {
                GetRowDaysContainer(startDate, finishDate).WriteTo(writer, HtmlEncoder.Default);
                result = writer.ToString();
            }

            return new HtmlString(result);
        }

        private static TagBuilder GetRowDaysContainer(DateTime startDate, DateTime finishDate)
        {
            TagBuilder rowBlock = new TagBuilder("div");
            rowBlock.AddCssClass("row");

            foreach (var tag in GetDaysContainer(startDate, finishDate))
            {
                rowBlock.InnerHtml.AppendHtml(tag);
            }
            
            return rowBlock;
        }

        private static IList<TagBuilder> GetDaysContainer(DateTime startDate, DateTime finishDate)
        {
            IList<TagBuilder> dayContainers = new List<TagBuilder>();

            for (DateTime date = startDate; date < finishDate; date = date.AddDays(1))
            {
                dayContainers.Add(GetDayBlockWithDate(date));
            }

            return dayContainers;
        }

        private static IList<TagBuilder> GetRowContainersList(
            int daysCount,
            IList<TagBuilder> dayContainers)
        {
            double rowCount = (double)daysCount / DaysInOneWeek;

            if (rowCount - (int)rowCount != 0)
            {
                rowCount = (int)rowCount + 1;
            }

            List<TagBuilder> rowContainers = new List<TagBuilder>();

            for (int i = 0; i < rowCount; i++)
            {
                rowContainers.Add(GetRowHtml(i, dayContainers));
            }

            return rowContainers;
        }

        //TODO: сделать по умолчанию вывод за неделю, либо по опр датам complited
        private static IList<TagBuilder> GetDaysContainersList(ref int daysCount, IList<CalendarScheduledEventViewModel> events, DateTime startDate, DateTime finishDate)
        {
            List<TagBuilder> dayContainers = new List<TagBuilder>();

            for (int day = 1; day <= daysCount; day++)
            {
                dayContainers.Add(GetDayContainerHtml(startDate.AddDays(day), events));
            }

            if (finishDate.DayOfWeek != DayOfWeek.Saturday)
            {
                int daysToAppendCount = DaysInOneWeek - (int)finishDate.DayOfWeek;

                int daysRangeLength = daysCount + daysToAppendCount; // why daysCount + daysToAppendCount;

                for (int day = daysCount + 1; day < daysRangeLength; day++)
                {
                    dayContainers.Add(GetDayBlockWithDateWithOutOfRange(startDate.AddDays(day)));
                }

                daysCount += daysToAppendCount;
            }

            if (startDate.AddDays(1).DayOfWeek != DayOfWeek.Sunday)
            {
                int daysToPrependCount = (int)startDate.DayOfWeek;

                for (int i = 0; i <= daysToPrependCount; i++)
                {
                    dayContainers.Insert(0, GetDayBlockWithDateWithOutOfRange(startDate.AddDays(-i)));
                }
            }

            return dayContainers;
        }

        private static TagBuilder GetRowHtml(int row, IList<TagBuilder> days)
        {
            int daysRangeLength = row * DaysInOneWeek + DaysInOneWeek;
            TagBuilder rowBlock = new TagBuilder("div");
            rowBlock.AddCssClass("row");

            if (daysRangeLength > days.Count)
            {
                daysRangeLength = days.Count;
            }

            for (int i = row * DaysInOneWeek; i < daysRangeLength; i++)
            {
                rowBlock.InnerHtml.AppendHtml(days[i]);
            }

            return rowBlock;
        }

        private static TagBuilder GetDayContainerHtml(DateTime current, IList<CalendarScheduledEventViewModel> events)
        {
            var eventsFiltered = events.Where(x => x.EventFinish.Date >= current.Date && x.EventStart.Date <= current.Date).ToList();

            return GetDateContainerHtml(current, eventsFiltered);
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

            ////////////
            //TagBuilder rowBlock = new TagBuilder("div");
            //rowBlock.AddCssClass("row");

            //rowBlock.InnerHtml.AppendHtml(dayBlock);

            return dayBlock;
        }

        private static TagBuilder GetDayBlock(DateTime date)
        {
            string rowClass = IsWeekend(date) ? "col-1" : "col-2";

            TagBuilder div = new TagBuilder("div");
            div.AddCssClass(rowClass);

            return div;
        }

        private static TagBuilder GetDateContainerHtml(DateTime day, IEnumerable<CalendarScheduledEventViewModel> events)
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

            foreach (var e in events)
            {
                TagBuilder button = GetButtonContainerHtml(e, btnClass);

                divEvents.InnerHtml.AppendHtml(button);
            }

            div.InnerHtml.AppendHtml(divEvents);

            return div;
        }

        private static TagBuilder GetButtonContainerHtml(CalendarScheduledEventViewModel model, string cssClass)
        {
            TagBuilder button = new TagBuilder("button");

            button.AddCssClass(cssClass);
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-toggle", "modal");
            button.Attributes.Add("data-target", "#seeSchedulEvent");
            button.Attributes.Add("seGroupId", model.StudentGroupId.ToString());
            button.Attributes.Add("seMentorId", model.MentorId.ToString());
            button.Attributes.Add("seLessonId", model.LessonId.ToString());
            button.Attributes.Add("seName", model.Name);
            button.Attributes.Add("seThemeId", model.ThemeId.ToString());
            button.InnerHtml.Append(model.Name);

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
}
