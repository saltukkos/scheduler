using System.IO;
using System.Net;

namespace Scheduler.ScheduleStealer
{
    public class WebDownloader : IWebDownloader
    {
        public Stream GetLoadStream(string url)
        {
            return WebRequest.Create(url).GetResponseAsync().Result.GetResponseStream();
        }
    }
}