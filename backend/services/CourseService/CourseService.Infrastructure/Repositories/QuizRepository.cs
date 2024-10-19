using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public QuizRepository(IMongoCollection<Quiz> collection) : base(collection) { }

        public async Task<long> CountByLessonIdAsync(string lessonId)
        {
            var filter = Builders<Quiz>.Filter.Eq(q => q.LessonId, lessonId);

            return await _collection.CountDocumentsAsync(filter);
        }

        public async Task<List<Quiz>> FindByLessonIdAsync(string lessonId)
        {
            var filter = Builders<Quiz>.Filter.Eq(q => q.LessonId, lessonId);

            return await _collection.Find(filter).SortBy(q => q.Number).ToListAsync();
        }

        public async Task ChangeOrderAsync(string lessonId, List<string> quizIds)
        {
            var filterBuilder = Builders<Quiz>.Filter;
            var updateBuilder = Builders<Quiz>.Update;
            var writes = new List<WriteModel<Quiz>>();

            var length = quizIds.Count;

            for (int index = 0; index < length; index++)
            {
                var quizId = quizIds[index];

                writes.Add(new UpdateOneModel<Quiz>(
                    filterBuilder.Eq(q => q.Id, quizId) & filterBuilder.Eq(q => q.LessonId, lessonId),
                    updateBuilder.Set(q => q.Number, index + 1)
                ));
            }

            if (writes.Count > 0) 
                await _collection.BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });
        }

        public async Task DeleteByLessonIdAsync(string lessonId)
        {
            await _collection.DeleteManyAsync(Builders<Quiz>.Filter.Eq(q => q.LessonId, lessonId));
        }

        public async Task DeleteByLessonIdsAsync(IEnumerable<string> lessonIds)
        {
            var filter = Builders<Quiz>.Filter.In(q => q.LessonId, lessonIds);
            await _collection.DeleteManyAsync(filter);
        }
    }
}
