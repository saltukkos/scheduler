using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Scheduler.ScheduleStealer;
using Scheduler.ScheduleStealer.Schedule;
using Scheduler.ScheduleStealer.UserSelections;

namespace ScheduleRunner
{
    static class Program
    {
        private const string BaseUri = "http://table.nsu.ru/group/";

        private static readonly List<string> Groups = new List<string>
        {
            "14201",
            "14202",
            "14203",
            "14204",
            "14205",
            "14206"
        };


        static void Main()
        {
            var httpClient = new HttpClient();
            if (!httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "Authorization", Environment.GetEnvironmentVariable("ScheduleStealerAuth")))
            {
                Console.Error.WriteLine("Can't add authorization");
            }


            var scheduleUpdater = new ScheduleUpdater(BaseUri, new ScheduleParser(), new WebDownloader());
            var studentSelectionUpdater = new StudentSelectionUpdater("http://fit.nsu.ru/uch/bak/4-kurs-raspredelenie-po-distsiplinam-po-vyboru", new WebDownloader());
            var individualScheduleBuilder = new IndividualScheduleBuilder(scheduleUpdater, studentSelectionUpdater);

            for (;;)
            {
                try
                {
                    var res = individualScheduleBuilder.UpdateIndividualSchedule(Groups);
                    var json = JsonConvert.SerializeObject(res);

                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    var httpResponseMessage = httpClient.PostAsync("https://nsutable.ru/api/schedule/", stringContent).Result;
                    Console.Out.WriteLine("{0:HH:mm:ss tt}: {1}", DateTime.Now, httpResponseMessage.Content.ReadAsStringAsync().Result);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("{0:HH:mm:ss tt}: {1}", DateTime.Now, e.Message);
                    Thread.Sleep(new TimeSpan(0, 5, 0));
                }
                if (Console.KeyAvailable && Console.ReadLine() == "exit")
                {
                    return;
                }
                Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }
    }
}