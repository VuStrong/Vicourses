﻿using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CourseService.Infrastructure.ClassMaps
{
    public class LessonMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<VideoFile>(map =>
            {
                map.AutoMap();
                map.MapMember(c => c.Status)
                    .SetSerializer(new EnumSerializer<VideoStatus>(BsonType.String));

                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Lesson>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);
                map.MapMember(c => c.Type)
                    .SetSerializer(new EnumSerializer<LessonType>(BsonType.String));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
