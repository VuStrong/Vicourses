using CourseService.Domain.Enums;
using CourseService.Domain.Events.Course;
using CourseService.Domain.Events.Enrollment;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;

namespace CourseService.Tests.UnitTests.Domain
{
    public class CourseModelTest
    {
        private Category _rootCategory;
        private Category _childCategory;
        private User _user;

        public CourseModelTest()
        {
            (_rootCategory, _childCategory) = PrepareRootChildCategory();

            _user = User.Create("UserId1", "ABC", "abc@gmail.com", null);
        }

        [Fact]
        public void Create_ShouldReturnCourse_WhenValid()
        {
            var title = "    Dotnet core course ";

            var course = Course.Create(title, null, _rootCategory, _childCategory, _user);

            Assert.NotNull(course);
            Assert.Equal(title.Trim(), course.Title);
        }

        [Fact]
        public void Create_ShouldThrow_WhenChildCategoryIsNotChildOfParentCategory()
        {
            typeof(Category).GetProperty("ParentId")!.SetValue(_childCategory, "AnotherParent", null);

            Assert.Throws<BusinessRuleViolationException>(() => Course.Create("course", null, _rootCategory, _childCategory, _user));
        }

        [Fact]
        public void SetStatus_Published_ShouldThrow_WhenCourseIsNotApproved()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);

            Assert.False(course.IsApproved);
            Assert.Equal(CourseStatus.Unpublished, course.Status);

            Assert.Throws<BusinessRuleViolationException>(() => course.SetStatus(CourseStatus.Published));
        }

        [Fact]
        public void EnrollStudent_ShouldThrow_WhenCourseIsUnpublished()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);

            Assert.Throws<BusinessRuleViolationException>(() => course.EnrollStudent("Student1"));
        }

        [Fact]
        public void EnrollStudent_ShouldRaiseDomainEvent()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);
            course.SetStatus(CourseStatus.WaitingToVerify);
            course.Approve();

            course.EnrollStudent("Student1");

            Assert.Contains(course.DomainEvents, e => e.GetType() == typeof(UserEnrolledDomainEvent));
        }

        [Fact]
        public void UpdateThumbnail_ShouldNotRaiseDomainEvent_WhenFileIdIsNotChanged()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);
            var fileId = "fileId";

            var thumbnail = ImageFile.Create(fileId, "url");
            course.UpdateThumbnail(thumbnail);

            Assert.NotNull(course.Thumbnail);
            Assert.Contains(course.DomainEvents, e => e.GetType() == typeof(CourseThumbnailUpdatedDomainEvent));

            var sameThumbnail = ImageFile.Create(fileId, "url2");
            course.ClearDomainEvents();
            course.UpdateThumbnail(sameThumbnail);

            Assert.Empty(course.DomainEvents);
        }

        [Fact]
        public void UpdatePreviewVideo_ShouldNotRaiseDomainEvent_WhenFileIdIsNotChanged()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);
            var videoId = "videoId";

            var previewVideo = VideoFile.Create(videoId, "url", "videoName");
            course.UpdatePreviewVideo(previewVideo);

            Assert.NotNull(course.PreviewVideo);
            Assert.Contains(course.DomainEvents, e => e.GetType() == typeof(CoursePreviewVideoUpdatedDomainEvent));

            var samePreviewVideo = VideoFile.Create(videoId, "url2", "videoName2");
            course.ClearDomainEvents();
            course.UpdatePreviewVideo(samePreviewVideo);

            Assert.Empty(course.DomainEvents);
        }

        [Fact]
        public void UpdateInfoIgnoreNull_ShouldRaiseDomainEvent_WhenUpdated()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);
            var title = "course 2  ";

            course.UpdateInfoIgnoreNull(title);

            Assert.Equal(title.Trim(), course.Title);
            Assert.Contains(course.DomainEvents, e => e.GetType() == typeof(CourseInfoUpdatedDomainEvent));
        }

        private (Category, Category) PrepareRootChildCategory()
        {
            var rootCategory = (Category)Activator.CreateInstance(typeof(Category), true)!;
            typeof(Category).GetProperty("Id")!.SetValue(rootCategory, "Root1", null);
            typeof(Category).GetProperty("ParentId")!.SetValue(rootCategory, null, null);

            var childCategory = (Category)Activator.CreateInstance(typeof(Category), true)!;
            typeof(Category).GetProperty("Id")!.SetValue(childCategory, "Child1", null);
            typeof(Category).GetProperty("ParentId")!.SetValue(childCategory, rootCategory.Id, null);

            return (rootCategory, childCategory);
        }
    }
}