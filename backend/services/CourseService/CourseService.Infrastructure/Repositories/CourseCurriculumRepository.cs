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
        private readonly IMongoCollection<Lession> _lessionCollection;

        public CourseCurriculumRepository(
            IMongoCollection<Section> sectionCollection,
            IMongoCollection<Lession> lessionCollection)
        {
            _sectionCollection = sectionCollection;
            _lessionCollection = lessionCollection;
        }

        public async Task<Section?> GetSectionByIdAsync(string id)
        {
            var filter = Builders<Section>.Filter.Eq(s => s.Id, id);

            return await _sectionCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Lession?> GetLessionByIdAsync(string id)
        {
            var filter = Builders<Lession>.Filter.Eq(l => l.Id, id);

            return await _lessionCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateSectionAsync(Section section)
        {
            await _sectionCollection.InsertOneAsync(section);
        }

        public async Task UpdateSectionAsync(Section section)
        {
            var filter = Builders<Section>.Filter.Eq(s => s.Id, section.Id);

            await _sectionCollection.ReplaceOneAsync(filter, section);
        }

        public async Task DeleteSectionAsync(string sectionId)
        {
            await _sectionCollection.DeleteOneAsync(Builders<Section>.Filter.Eq(s => s.Id, sectionId));
        }

        public async Task CreateLessionAsync(Lession lession)
        {
            await _lessionCollection.InsertOneAsync(lession);
        }

        public async Task UpdateLessionAsync(Lession lession)
        {
            var filter = Builders<Lession>.Filter.Eq(l => l.Id, lession.Id);

            await _lessionCollection.ReplaceOneAsync(filter, lession);
        }

        public async Task DeleteLessionAsync(string lessionId)
        {
            await _lessionCollection.DeleteOneAsync(Builders<Lession>.Filter.Eq(l => l.Id, lessionId));
        }

        public async Task<List<SectionWithLessions>> GetCourseCurriculumAsync(string courseId)
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
                        { "from", "lessions" },
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
                                            new BsonArray{ "$$id", $"${nameof(Lession.SectionId)}" }
                                        )
                                    )
                                ),
                                new BsonDocument(
                                    "$sort",
                                    new BsonDocument{ { $"{nameof(Lession.Order)}", 1 } }
                                )
                            }
                        },
                        { "as", nameof(SectionWithLessions.Lessions) }
                    }
                ),
                new BsonDocument("$addFields",
                    new BsonDocument
                    {
                        { $"{nameof(SectionWithLessions.Duration)}", new BsonDocument { { "$sum", "$Lessions.Duration" } } },
                        { $"{nameof(SectionWithLessions.LessionCount)}", new BsonDocument { { "$size", "$Lessions" } } }
                    }
                ),
                new BsonDocument("$sort",
                    new BsonDocument{ { $"{nameof(Section.Order)}", 1 } }
                )
            };

            var result = await _sectionCollection
                .Aggregate<SectionWithLessions>(pipeline)
                .ToListAsync();

            return result;
        }

        public async Task UpdateCurriculumAsync(string courseId, List<CurriculumItem> items)
        {
            (var sectionWrites, var lessionWrites) = BuildSectionAndLessionWriteOps(courseId, items);
            var bulkOptions = new BulkWriteOptions { IsOrdered = false };

            if (sectionWrites.Count > 0)
                await _sectionCollection.BulkWriteAsync(sectionWrites, bulkOptions);
            if (lessionWrites.Count > 0)
                await _lessionCollection.BulkWriteAsync(lessionWrites, bulkOptions);
        }

        private (List<WriteModel<Section>>, List<WriteModel<Lession>>) BuildSectionAndLessionWriteOps(string courseId, List<CurriculumItem> items)
        {
            var sectionWrites = new List<WriteModel<Section>>();
            var sectionFilter = Builders<Section>.Filter;
            var sectionUpdate = Builders<Section>.Update;

            var lessionWrites = new List<WriteModel<Lession>>();
            var lessionFilter = Builders<Lession>.Filter;
            var lessionUpdate = Builders<Lession>.Update;

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
                    UpdateDefinition<Lession>? update = null;

                    if (lastSectionId != string.Empty)
                    {
                        update = lessionUpdate.Set(l => l.Order, index - (sectionCount - 1))
                            .Set(l => l.SectionId, lastSectionId);
                    }
                    else
                    {
                        update = lessionUpdate.Set(l => l.Order, index - (sectionCount - 1));
                    }

                    lessionWrites.Add(new UpdateOneModel<Lession>(
                        lessionFilter.Eq(l => l.Id, item.Id) & lessionFilter.Eq(l => l.CourseId, courseId),
                        update
                    ));
                }
            }

            return (sectionWrites, lessionWrites);
        }
    }
}
