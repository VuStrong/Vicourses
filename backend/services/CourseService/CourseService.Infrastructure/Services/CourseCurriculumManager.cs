using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Services
{
    public class CourseCurriculumManager : ICourseCurriculumManager
    {
        private readonly IMongoCollection<Section> _sectionCollection;
        private readonly IMongoCollection<Lesson> _lessonCollection;

        public CourseCurriculumManager(
            IMongoCollection<Section> sectionCollection,
            IMongoCollection<Lesson> lessonCollection)
        {
            _sectionCollection = sectionCollection;
            _lessonCollection = lessonCollection;
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
