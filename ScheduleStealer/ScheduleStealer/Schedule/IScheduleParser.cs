using System.Collections.Generic;
using HtmlAgilityPack;

namespace Scheduler.ScheduleStealer.Schedule
{
    public interface IScheduleParser
    {
        IReadOnlyCollection<LessonInfo> ParseTable(HtmlDocument table, string group);
    }
}