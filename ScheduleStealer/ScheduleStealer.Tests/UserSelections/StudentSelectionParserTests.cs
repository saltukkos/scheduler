using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Scheduler.ScheduleStealer.UserSelections;

namespace Scheduler.ScheduleStealer.Tests.UserSelections
{
    [TestFixture]
    public class StudentSelectionParserTests
    {
        [Test]
        public void ParseCorrectPage()
        {
            var downloaderMock = new Mock<IWebDownloader>();
            downloaderMock.Setup(d => d.GetLoadStream(It.IsAny<string>()))
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(CorrectPage)));
            var studentSelectionParser = new StudentSelectionUpdater("123", downloaderMock.Object);

            var readOnlyList = studentSelectionParser.Update();
            Assert.That(readOnlyList.GroupBy(s => s.SpecCourseName).Count(), Is.EqualTo(4));
            Assert.That(readOnlyList.Count(s => s.Name == "Лихачев Александр Евгеньевич" 
                    && s.SpecCourseName == "Коллективная разработка программного обеспечения"
                    && s.Group == "14202"), Is.EqualTo(1));
        }

        [Test]
        public void ParseIncorrectPage()
        {
            var downloaderMock = new Mock<IWebDownloader>();
            downloaderMock.Setup(d => d.GetLoadStream(It.IsAny<string>()))
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes("<!doctype html><html>Some random text</html>")));
            var studentSelectionParser = new StudentSelectionUpdater("123", downloaderMock.Object);

            Assert.That(() => studentSelectionParser.Update(), Throws.ArgumentException);
        }


        private const string CorrectPage = @"
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""ru-ru"" lang=""ru-ru"" dir=""ltr"">
<head>
<table border=""1"" cellpadding=""0"" cellspacing=""0"" style=""width: 100%;"">
<tbody>
<tr height=""50"">
<td style=""color: white; background: MediumBlue; font-size: 14pt; text-align: center;""><strong>2017-2018 учебный год</strong></td>
</tr>
<tr height=""50"">
<td style=""color: white; background: MidnightBlue; font-size: 14pt; text-align: center;""><strong>Б3.В.ДВ.6</strong></td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>1. Введение в ГИС-технологии -- КафСИ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;"">Не состоится</td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>2. Продвинутое программирование на С# -- КафCИ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;"">Не состоится</td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>3. Коллективная разработка программного обеспечения -- КафОИ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;""><b>1 подгруппа</b><ol>
<li>Биндер Сергей Дмитриевич -- 14201</li>
<li>Викторов Николай Алексеевич -- 14201</li>
<li>Вишневский Роман Юрьевич -- 14201</li>
<li>Грачев Виталий Игоревич -- 14202</li>
<li>Игнатенко Татьяна Андреевна -- 14202</li>
<li>Кириченко Михаил Дмитриевич -- 14201</li>
<li>Липаткин Артем Евгеньевич -- 14201</li>
<li>Лихачев Александр Евгеньевич -- 14202</li>
<li>Лобков Илья Иванович -- 14201</li>
<li>Макалев Андрей Игоревич -- 14202</li>
</ol></td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>4. Модели и методы искусственного интеллекта -- КафСИ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;""><ol>
<li>Амикишиева Руслана Александровна -- 14206</li>
<li>Барсегян Артур Арсенович -- 14202</li>
<li>Бедарев Николай Андреевич -- 14204</li>
<li>Бычков Никита Михайлович -- 14202</li>
<li>Данилко Виталий Романович -- 14204</li>
<li>Демидов Сергей Евгеньевич -- 14206</li>
<li>Дудаев Александр Русланович -- 14206</li>
<li>Елисеев Иван Дмитриевич -- 14204</li>
<li>Катчик Никита Олегович -- 14206</li>
<li>Кондратьев Александр Юрьевич -- 14202</li>
<li>Кузьмичев Александр Валерьевич -- 14204</li>
<li>Липовый Дмитрий Анатольевич -- 14205</li>
<li>Малафеев Виталий Эдуардович -- 14206</li>
<li>Матвеев Андрей Олегович -- 14206</li>
<li>Найденов Александр Максимович -- 14203</li>
<li>Непомнящий Илья Андреевич -- 14204</li>
<li>Посохин Роман Вадимович -- 14205</li>
<li>Седухин Олег Андреевич -- 14204</li>
<li>Семушенко Елена Александровна -- 14201</li>
<li>Топольняк Егор -- 14205</li>
<li>Трофимова Екатерина Александровна -- 14203</li>
<li>Федин Артем Алексеевич -- 14202</li>
<li>Шаченко Никита Вадимович -- 14206</li>
<li>Ян Игорь Леонидович -- 14202</li>
</ol></td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>5. Моделирование деятельности предприятия -- КафСИ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;"">
<p>&nbsp;Не состоится</p>
</td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>6. Системы технического зрения -- КафКТ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;"">
<p>&nbsp;Не состоится</p>
</td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>7. Введение в компьютерное моделирование в науках о Земле -- КафСИ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;"">
<p>&nbsp;Не состоится</p>
</td>
</tr>
<tr height=""50"">
<td style=""color: white; background: MidnightBlue; font-size: 14pt; text-align: center;""><strong>Б3.В.ДВ.7</strong></td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>1. Введение в организацию распределенных вычислений -- КафПВ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;""><ol>
<li>Бедарев Николай Андреевич -- 14204</li>
<li>Данилко Виталий Романович -- 14204</li>
<li>Катчик Никита Олегович -- 14206</li>
<li>Провоторов Никита Владимирович -- 14204</li>
<li>Прокопьева Анастасия Валерьевна -- 14204</li>
<li>Самарин Роман Андреевич -- 14202</li>
<li>Шелехин Александр Викторович -- 14203</li>
</ol></td>
</tr>
<tr height=""40"">
<td style=""color: white; background: SteelBlue; font-size: 12pt; text-align: center;""><strong>2. Инжиниринг современных информационных систем -- КафКТ</strong></td>
</tr>
<tr height=""30"">
<td style=""background: LightCyan; font-size: 12pt; text-align: left;""><ol>
<li>Амикишиева Руслана Александровна -- 14206</li>
<li>Барсегян Артур Арсенович -- 14202</li>
<li>Викторов Николай Алексеевич -- 14201</li>
<li>Вишневский Роман Юрьевич -- 14201</li>
<li>Гаразовский Максим -- 14204</li>
<li>Грачев Виталий Игоревич -- 14202</li>
<li>Дробот Алёна Владимировна -- 14203</li>
<li>Игнатенко Татьяна Андреевна -- 14202</li>
<li>Лихачев Александр Евгеньевич -- 14202</li>
<li>Макалев Андрей Игоревич -- 14202</li>
<li>Мамеев Никита Сергеевич -- 14205</li>
<li>Трофимова Екатерина Александровна -- 14203</li>
<li>Чеплаков Михаил Михайлович -- 14205</li>
<li>Чернова Екатерина Вячеславовна -- 14201</li>
<li>Чирихин Александр Сергеевич -- 14201</li>
<li>Ян Игорь Леонидович -- 14202</li>
</ol></td>
</tr>
</tbody>
</table>
</div>			
</div>
</body>
</html>
";
    }
}