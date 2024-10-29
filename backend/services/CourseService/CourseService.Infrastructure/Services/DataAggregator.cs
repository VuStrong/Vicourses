using CourseService.Application.Dtos.Category;
using CourseService.Application.Dtos.Section;
using CourseService.Application.Interfaces;
using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Services
{
    public class DataAggregator : IDataAggregator
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Section> _sectionCollection;

        public DataAggregator(
            IMongoCollection<Category> categoryCollection,
            IMongoCollection<Section> sectionCollection
            )
        {
            _categoryCollection = categoryCollection;
            _sectionCollection = sectionCollection;
        }

        public async Task<List<CategoryWithSubsDto>> GetRootCategoriesWithSubCategoriesAsync()
        {
            var pipeline = new[] {
                new BsonDocument("$match",
                    new BsonDocument
                    {
                        { nameof(Category.ParentId), BsonNull.Value }
                    }
                ),
                new BsonDocument("$lookup",
                    new BsonDocument
                    {
                        { "from", "categories" },
                        { "localField", "_id" },
                        { "foreignField", nameof(Category.ParentId) },
                        { "as", nameof(CategoryWithSubsDto.SubCategories) }
                    }
                )
            };

            var result = await _categoryCollection
                .Aggregate<CategoryWithSubsDto>(pipeline)
                .ToListAsync();

            return result;
        }

        public async Task<List<SectionWithLessonsDto>> GetSectionsWithLessonsAsync(string courseId)
        {
            var pipeline = new[] {
                new BsonDocument("$match",
                    new BsonDocument
                    {
                        { nameof(Section.CourseId), courseId }
                    }
                ),
                new BsonDocument("$lookup",
                    new BsonDocument
                    {
                        { "from", "lessons" },
                        {
                            "let",
                            new BsonDocument
                            {
                                { "id", "$_id" }
                            }
                        },
                        {
                            "pipeline",
                            new BsonArray
                            {
                                new BsonDocument(
                                    "$match",
                                    new BsonDocument
                                    (
                                        "$expr",
                                        new BsonDocument(
                                            "$eq",
                                            new BsonArray{ "$$id", $"${nameof(Lesson.SectionId)}" }
                                        )
                                    )
                                ),
                                new BsonDocument(
                                    "$sort",
                                    new BsonDocument{ { $"{nameof(Lesson.Order)}", 1 } }
                                )
                            }
                        },
                        { "as", nameof(SectionWithLessonsDto.Lessons) }
                    }
                ),
                new BsonDocument("$addFields",
                    new BsonDocument
                    {
                        { $"{nameof(SectionWithLessonsDto.Duration)}", new BsonDocument { { "$sum", "$Lessons.Video.Duration" } } },
                        { $"{nameof(SectionWithLessonsDto.LessonCount)}", new BsonDocument { { "$size", "$Lessons" } } }
                    }
                ),
                new BsonDocument("$sort",
                    new BsonDocument{ { $"{nameof(Section.Order)}", 1 } }
                )
            };

            var result = await _sectionCollection
                .Aggregate<SectionWithLessonsDto>(pipeline)
                .ToListAsync();

            return result;
        }
    }
}
