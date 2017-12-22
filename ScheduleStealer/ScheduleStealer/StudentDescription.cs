using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Scheduler.ScheduleStealer.Schedule;

namespace Scheduler.ScheduleStealer
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class StudentDescription
    {
        public string Group { get; set; }

        public IReadOnlyCollection<LessonInfo> Lessons { get; set; }
    }
}