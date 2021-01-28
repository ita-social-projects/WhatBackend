using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandler : IScheduledEventHandler
    {
        private IScheduledEventHandler _content;

        public ScheduledEventHandler(EventOccurence source, ContextForCreateScheduleDTO context, PatternForCreateScheduleDTO pattern)
        {
            switch (source.Pattern)
            {
                case PatternType.Daily:
                    _content = new ScheduledEventHandlerDaily(source, context, pattern);
                    break;
                case PatternType.Weekly:
                    _content = new ScheduledEventHandlerDaily(source, context, pattern);
                    break;
                case PatternType.AbsoluteMonthly:
                    _content = new ScheduledEventHandlerAbsoluteMonthly(source, context, pattern);
                    break;
                case PatternType.RelativeMonthly:
                    _content = new ScheduledEventHandlerRelativeMonthly(source, context, pattern);
                    break;
                default:
                    break;
            }
        }

        public IEnumerable<ScheduledEvent> GetEvents()
        {
            return _content.GetEvents();
        }
    }
}
