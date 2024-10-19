using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseCurriculumRepository : ICourseCurriculumRepository
    {
        private readonly IMongoCollection<Section> _sectionCollection;
        private readonly IMongoCollection<Lesson> _lessonCollection;

        public CourseCurriculumRepository(
            IMongoCollection<Section> sectionCollection,
            IMongoCollection<Lesson> lessonCollection)
        {
            _sectionCollection = sectionCollection;
            _lessonCollection = lessonCollection;
        }

        public async Task<List<SectionWithLessons>> GetCourseCurriculumAsync(string courseId)
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
                        { "as", nameof(SectionWithLessons.Lessons) }
                    }
                ),
                new BsonDocument("$addFields",
                    new BsonDocument
                    {
                        { $"{nameof(SectionWithLessons.Duration)}", new BsonDocument { { "$sum", "$Lessons.Duration" } } },
                        { $"{nameof(SectionWithLessons.LessonCount)}", new BsonDocument { { "$size", "$Lessons" } } }
                    }
                ),
                new BsonDocument("$sort",
                    new BsonDocument{ { $"{nameof(Section.Order)}", 1 } }
                )
            };

            var result = await _sectionCollection
                .Aggregate<SectionWithLessons>(pipeline)
                .ToListAsync();

            return result;
        }

        public async Task UpdateCurriculumAsync(string courseId, List<CurriculumItem> items)
        {
            (var sectionWrites, var lessonWrites) = BuildSectionAndLessonWriteOps(courseId, items);
            var bulkOptions = new BulkWriteOptions { IsOrdered = false };

            if (sectionWrites.Count > 0)
                await _sectionCollection.BulkWriteAsync(sectionWrites, bulkOptions);
            if (lessonWrites.Count > 0)
                await _lessonCollection.BulkWriteAsync(lessonWrites, bulkOptions);
        }

        private (List<WriteModel<Section>>, List<WriteModel<Lesson>>) BuildSectionAndLessonWriteOps(string courseId, List<CurriculumItem> items)
        {
            var sectionWrites = new List<WriteModel<Section>>();
            var sectionFilter = Builders<Section>.Filter;
            var sectionUpdate = Builders<Section>.Update;

            var lessonWrites = new List<WriteModel<Lesson>>();
            var lessonFilter = Builders<Lesson>.Filter;
            var lessonUpdate = Builders<Lesson>.Update;

            var lastSectionId = string.Empty;
            var sectionCount = 0;
            int length = items.Count;

            for (int index = 0; index < length; index++)
            {
                var item = items[index];

                if (item.Type == CurriculumItemType.Section)
                {
                    sectionCount++;

                    sectionWrites.Add(new UpdateOneModel<Section>(
                        sectionFilter.Eq(s => s.Id, item.Id) & sectionFilter.Eq(s => s.CourseId, courseId),
                        sectionUpdate.Set(s => s.Order, sectionCount)
                    ));

                    lastSectionId = item.Id;
                }
                else
                {
                    UpdateDefinition<Lesson>? update = null;

                    if (lastSectionId != string.Empty)
                    {
                        update = lessonUpdate.Set(l => l.Order, index - (sectionCount - 1))
                            .Set(l => l.SectionId, lastSectionId);
                    }
                    else
                    {
                        update = lessonUpdate.Set(l => l.Order, index - (sectionCount - 1));
                    }

                    lessonWrites.Add(new UpdateOneModel<Lesson>(
                        lessonFilter.Eq(l => l.Id, item.Id) & lessonFilter.Eq(l => l.CourseId, courseId),
                        update
                    ));
                }
            }

            return (sectionWrites, lessonWrites);
        }
    }
}
