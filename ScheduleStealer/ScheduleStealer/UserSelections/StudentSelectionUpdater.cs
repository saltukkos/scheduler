using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Scheduler.ScheduleStealer.UserSelections
{
    public class StudentSelectionUpdater : IStudentSelectionUpdater
    {
        private readonly string _uri;
        private readonly IWebDownloader _webDownloader;

        public StudentSelectionUpdater(string uri, IWebDownloader webDownloader)
        {
            _uri = uri;
            _webDownloader = webDownloader;
        }

        public IReadOnlyList<StudentSelection> Update()
        {
            var result = new List<StudentSelection>();

            using (var stream = _webDownloader.GetLoadStream(_uri))
            {
                var document = new HtmlDocument();
                document.Load(stream, true);

                var htmlNodes = document.DocumentNode.Descendants("tbody");
                var node = htmlNodes.FirstOrDefault();
                if (node == null)
                    throw new ArgumentException("Не найдено ни одной таблицы");

                string lastSpec = null;
                foreach (var childNode in node.Descendants())
                {
                    if (childNode.Name == "strong")
                    {
                        TryFindSpecName(childNode.InnerText, ref lastSpec);
                    }

                    if (childNode.Name == "li")
                    {
                        if (lastSpec == null)
                        {
                            throw new ArgumentException("Список людей без предшествуещего имени спецкурса");
                        }

                        var pos = childNode.InnerText.IndexOf("--", StringComparison.Ordinal);
                        result.Add(new StudentSelection
                        {
                            Name = childNode.InnerText.Substring(0, pos - 1),
                            Group = childNode.InnerText.Substring(pos + 3, 5),
                            SpecCourseName = lastSpec
                        });
                    }
                }
            }

            return result;
        }

        private static void TryFindSpecName(string name, ref string spec)
        {
            var first = name.IndexOf(' ');
            var last = name.IndexOf("--", StringComparison.Ordinal);

            if (first == -1 || last == -1)
                return;

            spec = name.Substring(first + 1, last - first - 2);
        }
    }
}