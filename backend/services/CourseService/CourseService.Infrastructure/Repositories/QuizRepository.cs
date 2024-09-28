using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public QuizRepository(IMongoCollection<Quiz> collection) : base(collection) { }

        public async Task<long> CountByLessionIdAsync(string lessionId)
        {
            var filter = Builders<Quiz>.Filter.Eq(q => q.LessionId, lessionId);

            return await _collection.CountDocumentsAsync(filter);
        }

        public async Task<List<Quiz>> FindByLessionIdAsync(string lessionId)
        {
            var filter = Builders<Quiz>.Filter.Eq(q => q.LessionId, lessionId);

            return await _collection.Find(filter).SortBy(q => q.Number).ToListAsync();
        }

        public async Task ChangeOrderAsync(string lessionId, List<string> quizIds)
        {
            var filterBuilder = Builders<Quiz>.Filter;
            var updateBuilder = Builders<Quiz>.Update;
            var writes = new List<WriteModel<Quiz>>();

            var length = quizIds.Count;

            for (int index = 0; index < length; index++)
            {
                var quizId = quizIds[index];

                writes.Add(new UpdateOneModel<Quiz>(
                    filterBuilder.Eq(q => q.Id, quizId) & filterBuilder.Eq(q => q.LessionId, lessionId),
                    updateBuilder.Set(q => q.Number, index + 1)
                ));
            }

            if (writes.Count > 0) 
                await _collection.BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });
        }
    }
}
