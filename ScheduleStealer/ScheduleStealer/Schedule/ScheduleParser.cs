using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Scheduler.ScheduleStealer.Schedule
{
    public class ScheduleParser : IScheduleParser
    {

        public IReadOnlyCollection<LessonInfo> ParseTable(HtmlDocument table, string group)
        {
            var htmlNodes = table.DocumentNode.Descendants("table").Where(x => x.GetAttributeValue("class", null) != null);
            var node = htmlNodes.FirstOrDefault();
            if (node == null)
                throw new ArgumentException("Не найдено ни одной таблицы");

            var result = new List<LessonInfo>();

            var lessonNumber = 0;
            foreach (var htmlNode in node.ChildNodes.Where(x => x.Name == "tr").Skip(2))
            {
                result.AddRange(ParseLessons(htmlNode, ++lessonNumber, group));
            }

            return result;
        }

        private IEnumerable<LessonInfo> ParseLessons(HtmlNode htmlNode, int number, string group)
        {
            var htmlNodes = htmlNode.ChildNodes.Where(x => x.Name == "td").ToList();
            if (htmlNodes.Count != 7)
                throw new ArgumentException($"В строчке {number}-й пары {htmlNodes.Count} колонок вместо 7");

            var time = htmlNodes[0].InnerText;
            var resultList = new List<LessonInfo>();
            var n = 1;
            foreach (var node in htmlNodes.Skip(2))
            {
                var lesson = new LessonInfo
                {
                    Subject = node.ChildWithClass("subject")?.Attribute("title"),
                    IsLecture = node.ChildWithClass("type")?.Attribute("title") == "лекция",
                    Time = time,
                    Number = number,
                    WeekDay = n++,
                    IsOddWeek = DetectOdd(node.ChildWithClass("week")?.InnerText),
                    Room = node.ChildWithClass("room")?.InnerText,
                    Teacher = node.ChildWithClass("tutor")?.InnerText,
                    Group = group
                };

                if (lesson.Subject != null)
                {
                    resultList.Add(lesson);
                }
                if (lesson.Subject == null)
                { }
            }

            return resultList;
        }

        private bool? DetectOdd(string innerText)
        {
            if (innerText == null)
                return null;

            return innerText != "Нечетная";
        }
    }
}