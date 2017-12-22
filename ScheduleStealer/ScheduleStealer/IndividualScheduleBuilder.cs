using System.Collections.Generic;
using System.Linq;
using Scheduler.ScheduleStealer.Schedule;
using Scheduler.ScheduleStealer.UserSelections;

namespace Scheduler.ScheduleStealer
{
    public struct BuilderResult
    {
        public Dictionary<string, List<LessonInfo>> GroupsLessons { get; set; }
        public Dictionary<string, StudentDescription> StudentsLessons { get; set; }
    }

    public class IndividualScheduleBuilder
    {
        private readonly IScheduleUpdater _scheduleUpdater;
        private readonly IStudentSelectionUpdater _selectionUpdater;

        public IndividualScheduleBuilder(IScheduleUpdater scheduleUpdater, IStudentSelectionUpdater selectionUpdater)
        {
            _scheduleUpdater = scheduleUpdater;
            _selectionUpdater = selectionUpdater;
        }

        public BuilderResult UpdateIndividualSchedule(IReadOnlyList<string> groups)
        {
            var lessons = _scheduleUpdater.UpdateSchedule(groups);
            var selections = _selectionUpdater.Update();

            var specCourses = selections
                .Select(s => s.SpecCourseName)
                .Distinct()
                .ToList();

            var specCoursesInfo = lessons
                .Where(l => specCourses.Contains(l.Subject))
                .Distinct(new LessonsPositionComparer())
                .ToList();

            var studentLessons = selections
                .Select(s => new
                {
                    s.Name,
                    s.Group,
                    LessonInfo = specCoursesInfo
                        .Where(i => i.Subject == s.SpecCourseName)
                        .ToList()
                })
                .GroupBy(s => s.Name)
                .ToDictionary(
                    s => s.Key,
                    s => new StudentDescription
                    {
                        Group = s.First().Group,
                        Lessons = s.SelectMany(arr => arr.LessonInfo).ToList()
                    });

            var groupLessons = lessons
                .Where(l => !specCourses.Contains(l.Subject))
                .GroupBy(l => l.Group)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new BuilderResult
            {
                GroupsLessons = groupLessons,
                StudentsLessons = studentLessons
            };
        }

        private class LessonsPositionComparer : IEqualityComparer<LessonInfo>
        {
            public bool Equals(LessonInfo x, LessonInfo y)
            {
                return x.WeekDay == y.WeekDay
                       && x.Number == y.Number
                       && x.IsOddWeek == y.IsOddWeek
                       && x.Subject == y.Subject;
            }

            public int GetHashCode(LessonInfo obj)
            {
                unchecked
                {
                    var result = obj.Number;
                    result = (result * 397) ^ obj.WeekDay;
                    result = (result * 397) ^ obj.Subject.GetHashCode();
                    if (obj.IsOddWeek == true)
                    {
                        result += 1;
                    }
                    return result;
                }
            }
        }
    }
}