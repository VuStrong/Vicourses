using CourseService.Domain.Enums;
using CourseService.Domain.Events.Course;
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
        public void ShouldCreateCourseWithValidValues()
        {
            var title = "    Dotnet core course ";

            var course = Course.Create(title, null, _rootCategory, _childCategory, _user);

            Assert.NotNull(course);
            Assert.Equal(title.Trim(), course.Title);
        }

        [Fact]
        public void ShouldThrowIfCreateCourseWithSubCategoryIsNotChildOfParentCategory()
        {
            typeof(Category).GetProperty("ParentId")!.SetValue(_childCategory, "AnotherParent", null);

            Assert.Throws<DomainException>(() => Course.Create("course", null, _rootCategory, _childCategory, _user));
        }

        [Fact]
        public void TestCourseStatusLifeCycle()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);

            Assert.False(course.IsApproved);
            Assert.Equal(CourseStatus.Unpublished, course.Status);

            // Instructor trying to publish an unapproved course
            Assert.Throws<DomainException>(() => course.SetStatus(CourseStatus.Published));

            // Instructor send course to verify
            course.SetStatus(CourseStatus.WaitingToVerify);

            Assert.Equal(CourseStatus.WaitingToVerify, course.Status);

            // Admin approve it
            course.Approve();

            Assert.True(course.IsApproved);
            Assert.Equal(CourseStatus.Published, course.Status);

            // Instructor unpublish course and republish it
            course.SetStatus(CourseStatus.Unpublished);
            course.SetStatus(CourseStatus.Published);

            // Admin cancel approval of that course
            course.CancelApproval();

            Assert.False(course.IsApproved);
        }

        [Fact]
        public void ShouldThrowIfTryToEnrollUnpublishedCourse()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);

            Assert.Throws<DomainException>(() => course.EnrollStudent("Student1"));
        }

        [Fact]
        public void TestUpdateCourseThumbnailAndPreviewVideo()
        {
            var course = Course.Create("course", null, _rootCategory, _childCategory, _user);

            var thumbnail = ImageFile.Create("fileId", "url");
            course.UpdateThumbnail(thumbnail);

            Assert.NotNull(course.Thumbnail);
            Assert.Contains(course.DomainEvents, e => e.GetType() == typeof(CourseThumbnailUpdatedDomainEvent));

            var sameThumbnail = ImageFile.Create("fileId", "url2");
            course.ClearDomainEvents();
            course.UpdateThumbnail(sameThumbnail);

            Assert.Empty(course.DomainEvents);

            var previewVideo = VideoFile.Create("videoId", "url", "videoName");
            course.ClearDomainEvents();
            course.UpdatePreviewVideo(previewVideo);

            Assert.NotNull(course.PreviewVideo);
            Assert.Contains(course.DomainEvents, e => e.GetType() == typeof(CoursePreviewVideoUpdatedDomainEvent));

            var samePreviewVideo = VideoFile.Create("videoId", "url2", "videoName2");
            course.ClearDomainEvents();
            course.UpdatePreviewVideo(samePreviewVideo);

            Assert.Empty(course.DomainEvents);
        }

        [Fact]
        public void TestUpdateCourseRaiseDomainEvent()
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