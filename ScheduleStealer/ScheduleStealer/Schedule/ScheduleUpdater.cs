using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Scheduler.ScheduleStealer.Schedule
{
    public sealed class ScheduleUpdater : IScheduleUpdater
    {
        private readonly string _baseUri;
        private readonly IScheduleParser _parser;
        private readonly IWebDownloader _downloader;

        public ScheduleUpdater(string baseUri, IScheduleParser parser, IWebDownloader downloader)
        {
            _baseUri = baseUri;
            _parser = parser;
            _downloader = downloader;
        }

        public IReadOnlyList<LessonInfo> UpdateSchedule(IReadOnlyCollection<string> groups)
        {
            try
            {
                var lessons = new List<LessonInfo>();
                foreach (var group in groups)
                {
                    lessons.AddRange(GetGroupLessons(group));
                }

                return lessons;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private IReadOnlyCollection<LessonInfo> GetGroupLessons(string group)
        {
            var uri = $"{_baseUri}{group}";

            try
            {
                using (var stream = _downloader.GetLoadStream(uri))
                {
                    var document = new HtmlDocument();
                    document.Load(stream, true);
                    return _parser.ParseTable(document, group);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Проблема с группой {group}: {e.Message}");
                throw;
            }
        }

    }
}