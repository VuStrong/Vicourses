using CourseService.Domain.Enums;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;

namespace CourseService.Tests.UnitTests.Domain
{
    public class LessonModelTest
    {
        private Course _course;

        public LessonModelTest()
        {
            _course = PrepareCourse();
        }

        [Fact]
        public void Create_ShouldReturnLesson_WhenValid()
        {
            var section = Section.Create("sec1", _course, null);

            var title = "Lesson about dotnet core  ";
            var lesson = Lesson.Create(title, _course, section, LessonType.Video, null);

            Assert.NotNull(lesson);
            Assert.Equal(title.Trim(), lesson.Title);
            Assert.Equal(lesson.UserId, _course.User.Id);
            Assert.Equal(lesson.CourseId, _course.Id);
            Assert.Equal(lesson.SectionId, section.Id);
        }

        [Fact]
        public void Create_ShouldThrow_WhenSectionIsNotBelongToCourse()
        {
            var section = Section.Create("sec1", _course, null);
            typeof(Section).GetProperty("CourseId")!.SetValue(section, "OtherCourse", null);

            Assert.Throws<DomainValidationException>(() => Lesson.Create("les1", _course, section, LessonType.Video, null));
        }

        [Fact]
        public void UpdateVideo_ShouldNotRaiseDomainEvent_WhenFileIdIsNotChanged()
        {
            var section = Section.Create("sec1", _course, null);
            var lesson = Lesson.Create("les1", _course, section, LessonType.Video, null);
            var videoId = "videoId";

            var video = VideoFile.Create(videoId, "url", "videoName");
            lesson.UpdateVideo(video);

            Assert.NotNull(lesson.Video);
            Assert.Contains(lesson.DomainEvents, e => e.GetType() == typeof(LessonVideoUpdatedDomainEvent));

            var sameVideo = VideoFile.Create(videoId, "url2", "videoName2");
            lesson.ClearDomainEvents();
            lesson.UpdateVideo(sameVideo);

            Assert.Empty(lesson.DomainEvents);
        }

        [Fact]
        public void UpdateVideo_ShouldThrow_WhenLessonTypeIsNotVideo()
        {
            var section = Section.Create("sec1", _course, null);
            var lesson = Lesson.Create("les1", _course, section, LessonType.Quiz, null);
            var video = VideoFile.Create("id", "url", "name");

            Assert.Throws<DomainException>(() => lesson.UpdateVideo(video));
        }

        private Course PrepareCourse()
        {
            var rootCategory = (Category)Activator.CreateInstance(typeof(Category), true)!;
            typeof(Category).GetProperty("Id")!.SetValue(rootCategory, "Root1", null);
            typeof(Category).GetProperty("ParentId")!.SetValue(rootCategory, null, null);

            var childCategory = (Category)Activator.CreateInstance(typeof(Category), true)!;
            typeof(Category).GetProperty("Id")!.SetValue(childCategory, "Child1", null);
            typeof(Category).GetProperty("ParentId")!.SetValue(childCategory, rootCategory.Id, null);

            var user = User.Create("us", "us1", "u@gmail.com", null);

            return Course.Create("course", null, rootCategory, childCategory, user);
        }
    }
}
