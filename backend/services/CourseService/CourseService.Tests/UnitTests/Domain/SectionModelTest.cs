using CourseService.Domain.Models;

namespace CourseService.Tests.UnitTests.Domain
{
    public class SectionModelTest
    {
        private Course _course;

        public SectionModelTest()
        {
            _course = PrepareCourse();
        }

        [Fact]
        public void Create_ShouldReturnSection_WhenValid()
        {
            var title = "  Getting started  ";

            var section = Section.Create(title, _course, null);

            Assert.NotNull(section);
            Assert.Equal(title.Trim(), section.Title);
            Assert.Equal(section.UserId, _course.User.Id);
            Assert.Equal(section.CourseId, _course.Id);
        }

        [Fact]
        public void UpdateInfoIgnoreNull_ShouldUpdateValues()
        {
            var title = "  sec1";
            var desc = "This is a desc";
            var section = Section.Create(title, _course, null);

            section.UpdateInfoIgnoreNull(description: desc);

            Assert.Equal(title.Trim(), section.Title);
            Assert.Equal(desc, section.Description);
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
