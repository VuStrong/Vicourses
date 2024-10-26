using CourseService.Domain.Contracts;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;
using CourseService.Domain.Services.Implementations;
using NSubstitute;
using System.Linq.Expressions;

namespace CourseService.Tests.UnitTests.Domain
{
    public class CategoryModelTest
    {
        private ICategoryRepository _categoryRepository;
        private ICourseRepository _courseRepository;

        public CategoryModelTest()
        {
            _categoryRepository = Substitute.For<ICategoryRepository>();
            _courseRepository = Substitute.For<ICourseRepository>();
        }

        [Fact]
        public async Task ShouldThrowIfParentCategoryIsNotRoot()
        {
            _categoryRepository.ExistsAsync().Returns(false);

            var categoryDomainService = new CategoryDomainService(_categoryRepository, _courseRepository);

            var parentCate = await categoryDomainService.CreateAsync("parent", null);
            var childCate = await categoryDomainService.CreateAsync("child", parentCate);

            await Assert.ThrowsAsync<DomainException>(() => categoryDomainService.CreateAsync("child2", childCate));
        }

        [Fact]
        public async Task ShouldThrowIfTryToUpdateInUsedCategory()
        {
            _categoryRepository.ExistsAsync().Returns(false);
            _courseRepository.ExistsAsync(Arg.Any<Expression<Func<Course, bool>>>()).Returns(Task.FromResult(true));
            
            var categoryDomainService = new CategoryDomainService(_categoryRepository, _courseRepository);

            var category = await categoryDomainService.CreateAsync("cate", null);

            await Assert.ThrowsAsync<DomainException>(() => categoryDomainService.UpdateAsync(category, "name"));
        }
    }
}
