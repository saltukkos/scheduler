using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Scheduler.ScheduleStealer.Schedule;

namespace Scheduler.ScheduleStealer.Tests.Schedule
{
    [TestFixture]
    public class ScheduleParserTests
    {
        [Test]
        public void ParseCorrectPage()
        {
            var scheduleParser = new ScheduleParser();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(CorrectPageWithFifteenLessons);

            var list = scheduleParser.ParseTable(htmlDocument, "1");

            Assert.That(list, Is.Not.Empty);
            Assert.That(list.Count, Is.EqualTo(8));
            Assert.That(list.All(i => i.Teacher != null && i.Subject != null), Is.True);
            Assert.That(list.All(i => i.Group == "1"), Is.True);
        }

        [Test]
        public void ParseIncorrectPage()
        {
            var scheduleParser = new ScheduleParser();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<!doctype html><html>Some random text</html>");

            Assert.That(() => scheduleParser.ParseTable(htmlDocument, ""), Throws.ArgumentException.And.Message.Contains("таблиц"));
        }


        private const string CorrectPageWithFifteenLessons = @"
<!doctype html>
<html lang=""ru"">
<head>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <link rel=""shortcut icon"" href=""/img/favicon.ico"" type=""image/x-icon"">


    <link rel=stylesheet href=""/css/bootstrap.min.css"">
    <link rel=stylesheet href=""/css/style.css?v11"">
    <script src=""/js/jquery.min.js""></script>
    <script src='/js/bootstrap.min.js'></script>
    <link rel=stylesheet href='/css/schedule_group.css?v2'><link rel=stylesheet href='/css/print.css?v3' media='print'>    <link rel=icon href=""/img/favicon.ico"">
    <meta charset=""utf-8"">
    <title>Группа 14202 | Расписание занятий НГУ</title>

    <meta property=""og:title"" content=""Расписание занятий | Новосибирский государственный университет"">
    <meta property=""og:url"" content=""http://table.nsu.ru/"">
    <meta property=""og:type"" content=""website"">
    <meta property=""og:description"" name=""description"" content=""table.nsu.ru"">
    <meta property=""og:image"" content=""http://table.nsu.ru/img/opengraf.jpg"">
</head>
<body>
<header>
    <div class=""container"">
        <div class=""container-header"">
            <div>
                <a href=""http://www.nsu.ru/"" class=""logo-nsu"" target=""_blank""></a>
            </div>
            <div class=""title"">
                <a class=""text"" href=""/"">
                    РАСПИСАНИЕ&nbsp;ЗАНЯТИЙ&nbsp;НГУ
                </a>
                <div class=""parity"">
                    Четная неделя
                </div>
            </div>
            <div class=""text-update"">
                Дата последнего обновления                <div class=""date-update"">Сегодня в 17:42</div>
                <div class=""remember"">
                    <div><div class='info-remember'>Запомнить это расписание</div> <a class='btn-remember' href='/remember' data-toggle='tooltip' data-placement='right' title='Установить данное расписание по умолчанию. При следующем посещении сайта расписание отобразится автоматически.'></a></div>                </div>
            </div>
        </div>
    </div>
</header>
<div class=""container-nav"">
    <div class=""container"">
        <div>
            <ul class=""nav navbar-nav navbar-nsu"">
                <li>
                    <a href=""/faculties"">Факультеты</a>
                </li>
                <li>
                    <a href=""/teacher"">Преподаватели</a>
                </li>
                <li>
                    <a href=""/classes"">Компьютерные классы</a>
                </li>
            </ul>
        </div>
        <div class=""container-search pull-right"">
            <form action=""/search"" method=""get"">
                <input type=""text"" placeholder=""Поиск..."" name=""r"" required>
                <button type=""submit"" class=""btn""><i class=""icon-search""></i></button>
            </form>
        </div>
        <div class=""btn-group pull-right ring"">
            <a class=""btn btn-secondary dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
                <div class=""icon-ring""></div>
                <span>Расписание звонков</span>
            </a>
            <div class=""dropdown-menu dropdown-menu-right"">
                <table>
                    <tr>
                        <td><b>1 пара</b></td>
                        <td>9:00-9:45<br>9:50-10:35</td>
                    </tr>
                    <tr>
                        <td><b>2 пара</b></td>
                        <td>10:50-11:35<br>11:40-12:25</td>
                    </tr>
                    <tr>
                        <td><b>3 пара</b></td>
                        <td>12:40-13:25<br>13:30-14:15</td>
                    </tr>
                    <tr>
                        <td><b>4 пара</b></td>
                        <td>14:30-15:15<br>15:20-16:05</td>
                    </tr>
                    <tr>
                        <td><b>5 пара</b></td>
                        <td>16:20-17:05<br>17:10-17:55</td>
                    </tr>
                    <tr>
                        <td><b>6 пара</b></td>
                        <td>18:10-18:55<br>19:00-19:45</td>
                    </tr>
                    <tr>
                        <td><b>7 пара</b></td>
                        <td>20:00-20:45<br>20:50-21:35</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<div class=""container"">
        <ul class=""breadcrumb"">
        <li><a href=""/"" class=""icon-home""></a></li>
        <li><a href=""/"">Расписание</a></li>
        <li><a href=""/faculties"">Расписание по факультетам</a></li>
        <li><a href=""/faculty/fit"">Факультет информационных технологий</a></li>
        <li class=""active"">Группа 14202</li>
    </ul>
    <div class=""main_head"">
        <a href=""/faculty/fit"" class=""button""><div><span>◀</span> Назад</div></a>
        <div class=""nav-print""><div class=""icon-print""></div><a id=""print"">Распечатать расписание</a></div>
        <h1>Группа 14202</h1>
    </div>
		<table class=""time-table"" cellspacing=""0"">
            <col class=""col1"">
            <col span=""6"" class=""coln"">
			<tr>
				<th>Время</th>
				<th>Понедельник</th>
				<th>Вторник</th>
				<th>Среда</th>
				<th>Четверг</th>
				<th>Пятница</th>
				<th>Суббота</th>
			</tr>
			<tr><td>9:00</td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Защита информации'>Защита информ.</div><div class='room'>Ауд. 2128</div><a href='/teacher/001835' class='tutor'>Пермяков Р.А.</a></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'><span class='type pr' data-toggle='tooltip' data-placement='right' title='практика'>пр</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Управление производственным процессом разработки программного обеспечения'>УППРПО</div><div class='room'>Ауд. т4218</div><a href='/teacher/040100' class='tutor'>Исмагилов Т.З.</a></div></td><td><div class='cell'><span class='type pr' data-toggle='tooltip' data-placement='right' title='практика'>пр</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Экономика'>Экономика</div><div class='room'>Ауд. 5211</div><a href='/teacher/003277' class='tutor'>Есикова Т.Н.</a><div class='week'>Нечетная</div></div></td></tr><tr><td>10:50</td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Технологическое предпринимательство'>Технол.предприн.</div><div class='room'>Ауд. 2128</div><a href='/teacher/002197' class='tutor'>Васючкова Т.С.</a></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'><span class='type pr' data-toggle='tooltip' data-placement='right' title='практика'>пр</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Защита информации'>Защита информ.</div><div class='room'>Ауд. т4218</div><a href='/teacher/2871fba6-6b8c-11e7-81c4-005056af65d6' class='tutor'>Балабанов А.А.</a></div></td><td><div class='cell'></div></td></tr><tr><td>12:40</td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Экономика'>Экономика</div><div class='room'>Ауд. 2128</div><a href='/teacher/003277' class='tutor'>Есикова Т.Н.</a></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td></tr><tr><td>14:30</td><td><div class='cell'><span class='type pr' data-toggle='tooltip' data-placement='right' title='практика'>пр</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Метрология, стандартизация и сертификация'>Метрология</div><div class='room'>Ауд. т257 ГК</div><a href='/teacher/080362' class='tutor'>Держо М.А.</a></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Метрология, стандартизация и сертификация'>Метрология</div><div class='room'>Ауд. 2128</div><a href='/teacher/080362' class='tutor'>Держо М.А.</a></div></td><td><div class='cell'></div></td></tr><tr><td>16:20</td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Оптимизация производительности Java-программ'>ОП Java-программ</div><div class='room'>Ауд. 1156</div><a href='/teacher/004241' class='tutor'>Адаманский А.В.</a><div class='week'>Нечетная</div></div></td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Коллективная разработка программного обеспечения'>КРПО</div><div class='room'>Ауд. 2128</div><a href='/teacher/040091' class='tutor'>Мухортов В.В.</a></div></td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Управление производственным процессом разработки программного обеспечения'>УППРПО</div><div class='room'>Ауд. 2128</div><a href='/teacher/070007' class='tutor'>Анойкин Д.А.</a></div></td><td><div class='cell'><span class='type lek' data-toggle='tooltip' data-placement='right' title='лекция'>лек</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Инжиниринг современных информационных систем'>ИСИС</div><div class='room'>Ауд. Институт автоматики и электрометрии</div><a href='/teacher/003469' class='tutor'>Зюбин В.Е.</a></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td></tr><tr><td>18:10</td><td><div class='cell'><span class='type pr' data-toggle='tooltip' data-placement='right' title='практика'>пр</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Оптимизация производительности Java-программ'>ОП Java-программ</div><div class='room'>Ауд. 1156</div><a href='/teacher/004241' class='tutor'>Адаманский А.В.</a></div></td><td><div class='cell'><span class='type pr' data-toggle='tooltip' data-placement='right' title='практика'>пр</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Коллективная разработка программного обеспечения'>КРПО</div><div class='room'>Ауд. 2128</div><a href='/teacher/040091' class='tutor'>Мухортов В.В.</a></div></td><td><div class='cell'></div></td><td><div class='cell'><span class='type pr' data-toggle='tooltip' data-placement='right' title='практика'>пр</span><div class='subject' data-toggle='tooltip' data-placement='top' title='Инжиниринг современных информационных систем'>ИСИС</div><div class='room'>Ауд. Институт автоматики и электрометрии</div><a href='/teacher/003469' class='tutor'>Зюбин В.Е.</a></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td></tr><tr><td>20:00</td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td><td><div class='cell'></div></td></tr>		</table>
    <script>
        $(function () {
            $('[data-toggle=""tooltip""]').tooltip();
        })
    </script>

	</div>
<footer>
    <div class=""container"">
        <div class=""container-footer"">
            <div class=""info"">
                <a href=""http://www.nsu.ru/"" class=""logo-nsu2"" target=""_blank""></a>
                <p>
                    © 2017 Новосибирский государственный университет
                </p>
            </div>
            <div class=""contact"">
                <p>
                    Телефон: +7 (383) 363-41-50
                </`p>
                <p>
                    E-mail: <a href=""mailto:support@nsu.ru"">support@nsu.ru</a>
                </p>
                <p>
                    Адрес: Новосибирск, ул. Пирогова, 1
                </p>
            </div>
            <div class=""menu-footer"">
                <a href=""/faculties"">Расписание по факультетам</a>
                <a href=""/teacher"">Расписание преподавателей</a>
                <a href=""/classes"">Расписание компьютерных классов</a>

                <br>
                <a href=""https://appmetrica.yandex.com/serve/24485252021927636""><div class=""icon-apple""></div>App Store</a>
                <a href=""https://appmetrica.yandex.com/serve/673003597859934701""><div class=""icon-google""></div>Google Play</a>
            </div>
            <div class=""app-footer"">
            </div>
        </div>
    </div>
</footer>
</body>
</html>";
    }
}