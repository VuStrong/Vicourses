﻿using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CourseService.Infrastructure.ClassMaps
{
    public class LessonMap
    {
        public static void Configure()
        {
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
