using System.Diagnostics.CodeAnalysis;

namespace Scheduler.ScheduleStealer.Schedule
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public sealed class LessonInfo
    {
        /*группа, к которой принадлежит эта пара*/
        public string Group { get; set; }
        
        /*имя предмета*/
        public string Subject { get; set; }
        
        /*преподаватель*/
        public string Teacher { get; set; }

        /*аудитория*/
        public string Room { get; set; }

        /*Лекция?*/
        public bool IsLecture { get; set; }

        /*Пара проводится по нечётным неделям? null - не зависит от недели*/
        public bool? IsOddWeek { get; set; }

        /*День в неделе*/
        public int WeekDay { get; set; }

        /*Время проведения пары*/
        public string Time { get; set; }

        /*Номер пары по порядку*/
        public int Number { get; set; }
    }
}