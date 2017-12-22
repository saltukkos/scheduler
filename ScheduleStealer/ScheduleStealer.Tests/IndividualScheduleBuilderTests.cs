using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Scheduler.ScheduleStealer.Schedule;
using Scheduler.ScheduleStealer.UserSelections;

namespace Scheduler.ScheduleStealer.Tests
{
    [TestFixture]
    public class IndividualScheduleBuilderTests
    {
        [Test]
        public void GroupsLessonsCorrect()
        {
            var groups = new []{"1", "2"};
            var scheduleUpdaterMock = new Mock<IScheduleUpdater>();
            var selectionUpdaterMock = new Mock<IStudentSelectionUpdater>();

            var lessonInfo1 = new LessonInfo
            {
                Subject = "l1",
                Group = "g1"
            };

            var lessonInfo2 = new LessonInfo
            {
                Subject = "l2",
                Group = "g2"
            };
            var lessonInfo3 = new LessonInfo
            {
                Subject = "l3",
                Group = "g1"
            };

            scheduleUpdaterMock.Setup(x => x.UpdateSchedule(It.Is<string[]>(s => s == groups))).Returns(new []{lessonInfo1, lessonInfo2, lessonInfo3 });
            selectionUpdaterMock.Setup(x => x.Update()).Returns(new List<StudentSelection>());

            var individualScheduleBuilder = new IndividualScheduleBuilder(scheduleUpdaterMock.Object, selectionUpdaterMock.Object);
            var updateIndividualSchedule = individualScheduleBuilder.UpdateIndividualSchedule(groups);

            Assert.That(updateIndividualSchedule.StudentsLessons, Is.Empty);
            Assert.That(updateIndividualSchedule.GroupsLessons, Contains.Key(lessonInfo1.Group));
            Assert.That(updateIndividualSchedule.GroupsLessons, Contains.Key(lessonInfo2.Group));

            CollectionAssert.AreEquivalent(new []{lessonInfo1, lessonInfo3}, updateIndividualSchedule.GroupsLessons[lessonInfo1.Group]);
            CollectionAssert.AreEquivalent(new []{lessonInfo2}, updateIndividualSchedule.GroupsLessons[lessonInfo2.Group]);
        }

        [Test]
        public void GroupLessonsExcludeIndividual()
        {
            var groups = new[] { "1", "2" };
            var scheduleUpdaterMock = new Mock<IScheduleUpdater>();
            var selectionUpdaterMock = new Mock<IStudentSelectionUpdater>();

            var lessonInfo1 = new LessonInfo
            {
                Subject = "l1",
                Group = "g1"
            };

            var lessonInfo2 = new LessonInfo
            {
                Subject = "l2",
                Group = "g2"
            };
            var lessonInfo3 = new LessonInfo
            {
                Subject = "l3",
                Group = "g1"
            };

            scheduleUpdaterMock.Setup(x => x.UpdateSchedule(It.Is<string[]>(s => s == groups)))
                .Returns(new[]
                {
                    lessonInfo1,
                    lessonInfo2,
                    lessonInfo3
                });

            selectionUpdaterMock.Setup(x => x.Update())
                .Returns(new List<StudentSelection>
                {
                    new StudentSelection
                    {
                        Group = "g1",
                        Name = "name",
                        SpecCourseName = lessonInfo1.Subject
                    }
                });

            var individualScheduleBuilder = new IndividualScheduleBuilder(scheduleUpdaterMock.Object, selectionUpdaterMock.Object);
            var updateIndividualSchedule = individualScheduleBuilder.UpdateIndividualSchedule(groups);

            Assert.That(updateIndividualSchedule.GroupsLessons, Contains.Key(lessonInfo1.Group));
            Assert.That(updateIndividualSchedule.GroupsLessons, Contains.Key(lessonInfo2.Group));

            CollectionAssert.AreEquivalent(new[] { lessonInfo3 }, updateIndividualSchedule.GroupsLessons[lessonInfo1.Group]);
            CollectionAssert.AreEquivalent(new[] { lessonInfo2 }, updateIndividualSchedule.GroupsLessons[lessonInfo2.Group]);
        }

        [Test]
        public void IndividualTasksCorrect()
        {
            var groups = new[] { "1", "2" };
            var scheduleUpdaterMock = new Mock<IScheduleUpdater>();
            var selectionUpdaterMock = new Mock<IStudentSelectionUpdater>();

            var lessonInfo1 = new LessonInfo
            {
                Subject = "l1",
                Group = "g1"
            };

            var lessonInfo2 = new LessonInfo
            {
                Subject = "l2",
                Group = "g2"
            };
            var lessonInfo3 = new LessonInfo
            {
                Subject = "l3",
                Group = "g1"
            };

            scheduleUpdaterMock.Setup(x => x.UpdateSchedule(It.Is<string[]>(s => s == groups)))
                .Returns(new[]
                {
                    lessonInfo1,
                    lessonInfo2,
                    lessonInfo3
                });

            var studentSelection1 = new StudentSelection
            {
                Group = "g1",
                Name = "name",
                SpecCourseName = lessonInfo3.Subject
            };
            var studentSelection2 = new StudentSelection
            {
                Group = studentSelection1.Group,
                Name = studentSelection1.Name,
                SpecCourseName = lessonInfo1.Subject
            };
            selectionUpdaterMock.Setup(x => x.Update())
                .Returns(new List<StudentSelection>
                {
                    studentSelection1,
                    studentSelection2
                });

            var individualScheduleBuilder = new IndividualScheduleBuilder(scheduleUpdaterMock.Object, selectionUpdaterMock.Object);
            var updateIndividualSchedule = individualScheduleBuilder.UpdateIndividualSchedule(groups);

            Assert.That(updateIndividualSchedule.StudentsLessons.Count, Is.EqualTo(1));
            Assert.That(updateIndividualSchedule.StudentsLessons, Contains.Key(studentSelection1.Name));
            Assert.That(updateIndividualSchedule.StudentsLessons.First().Value.Lessons, Contains.Item(lessonInfo1));
            Assert.That(updateIndividualSchedule.StudentsLessons.First().Value.Lessons, Contains.Item(lessonInfo3));

            Assert.That(updateIndividualSchedule.GroupsLessons, Does.Not.ContainKey(lessonInfo1.Group));
            Assert.That(updateIndividualSchedule.GroupsLessons, Contains.Key(lessonInfo2.Group));

            Assert.That(updateIndividualSchedule.GroupsLessons.Count, Is.EqualTo(1));

            CollectionAssert.AreEquivalent(new[] { lessonInfo2 }, updateIndividualSchedule.GroupsLessons[lessonInfo2.Group]);
        }
    }
}