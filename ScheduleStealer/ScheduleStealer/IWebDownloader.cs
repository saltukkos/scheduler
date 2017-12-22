using System.IO;

namespace Scheduler.ScheduleStealer
{
    public interface IWebDownloader
    {
        Stream GetLoadStream(string url);
    }
}