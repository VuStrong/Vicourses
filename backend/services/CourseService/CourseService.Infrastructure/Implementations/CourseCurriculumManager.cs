using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Implementations
{
    public class CourseCurriculumManager : ICourseCurriculumManager
    {
        private readonly IMongoCollection<Section> _sectionCollection;
        private readonly IMongoCollection<Lession> _lessionCollection;

        public CourseCurriculumManager(
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

        public async Task CreateLessionAsync(Lession lession)
        {
            await _lessionCollection.InsertOneAsync(lession);
        }

        public async Task UpdateCurriculumAsync(List<CurriculumItem> items)
        {
            List<string> sectionIds = [];
            List<string> lessionIds = [];

            foreach (var item in items)
            {
                if (item.Type == CurriculumItemType.Section)
                {
                    sectionIds.Add(item.Id);
                }
                else
                {
                    lessionIds.Add(item.Id);
                }
            }

            await PerformUpdateSections(sectionIds);


        }

        private async Task PerformUpdateSections(List<string> sectionIds)
        {
            var sectionFilter = Builders<Section>.Filter.In(s => s.Id, sectionIds);
            var bsonArrayOfIds = new BsonArray(sectionIds);

            var updateDefinition = Builders<Section>.Update.Pipeline(new BsonDocument[]
            {
                new BsonDocument
                {
                    {
                        "$set",
                        new BsonDocument
                        {
                            {
                                "Order",
                                new BsonDocument
                                {
                                    {
                                        "$indexOfArray",
                                        new BsonArray
                                        {
                                            bsonArrayOfIds,
                                            "$_id"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });

            await _sectionCollection.UpdateManyAsync(sectionFilter, updateDefinition);
        }
    }
}
