using System.Collections.Generic;

namespace Scheduler.ScheduleStealer.Schedule
{
    public interface IScheduleUpdater
    {
        IReadOnlyList<LessonInfo> UpdateSchedule(IReadOnlyCollection<string> groups);
    }
}