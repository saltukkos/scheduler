using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using Scheduler.ScheduleStealer.Schedule;

namespace Scheduler.ScheduleStealer.Tests.Schedule
{
    [TestFixture]
    public class ScheduleUpdaterTests
    {
        [Test]
        public void DownloadAllGroupsSchedule()
        {
            const string baseUri = "BASE_URI/";

            var parserMock = new Mock<IScheduleParser>();
            parserMock
                .Setup(p => p.ParseTable(It.IsAny<HtmlDocument>(), It.IsAny<string>()))
                .Returns(new List<LessonInfo>());

            var downloaderMock = new Mock<IWebDownloader>();
            downloaderMock.Setup(d => d.GetLoadStream(It.IsAny<string>())).Returns(() => new MemoryStream());

            var scheduleUpdater = new ScheduleUpdater(baseUri, parserMock.Object, downloaderMock.Object);

            scheduleUpdater.UpdateSchedule(new[] {"1", "2", "3"});

            downloaderMock.Verify(d => d.GetLoadStream(It.Is<string>(s => s == baseUri + "1")), Times.Once);
            downloaderMock.Verify(d => d.GetLoadStream(It.Is<string>(s => s == baseUri + "2")), Times.Once);
            downloaderMock.Verify(d => d.GetLoadStream(It.Is<string>(s => s == baseUri + "3")), Times.Once);
        }

        [Test]
        public void PassCorrectData()
        {
            var parserMock = new Mock<IScheduleParser>();
            parserMock
                .Setup(p => p.ParseTable(It.IsAny<HtmlDocument>(), It.IsAny<string>()))
                .Returns(new List<LessonInfo>());

            var downloaderMock = new Mock<IWebDownloader>();
            var data = @"
<!DOCTYPE html>
<html>
<body>
<h1>My First Heading</h1>
</body>
</html>";
            downloaderMock.Setup(d => d.GetLoadStream(It.IsAny<string>()))
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(data)));

            var scheduleUpdater = new ScheduleUpdater("Some uri", parserMock.Object, downloaderMock.Object);

            scheduleUpdater.UpdateSchedule(new[] { "1"});

            parserMock.Verify(p => p.ParseTable(It.Is<HtmlDocument>(d => d.ParsedText == data), It.Is<string>(s => s == "1")), Times.Once);
        }
    }
}