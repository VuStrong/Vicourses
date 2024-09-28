﻿using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Objects;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Lession : IBaseEntity
    {
        public string Id { get; private set; }
        public string CourseId { get; private set; }
        public string SectionId { get; private set; }
        public string UserId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int Duration { get; private set; }
        public int Order {  get; private set; }
        public LessionType Type { get; private set; } = LessionType.Video;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public VideoFile? Video { get; private set; }

        private Lession(string id, string title, string courseId, string sectionId, string userId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
            SectionId = sectionId;
            UserId = userId;
        }

        public static Lession Create(string title, string courseId, string sectionId, string userId, LessionType type, string? description)
        {
            var id = StringExtensions.GenerateIdString(14);

            return new Lession(id, title, courseId, sectionId, userId)
            {
                Description = description,
                Type = type,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        public void UpdateInfoIgnoreNull(string? title = null, string? description = null, int? duration = null, VideoFile? video = null)
        {
            if (title != null) Title = title;
            if (description != null) Description = description;
            if (video != null) Video = video;
            if (duration != null) Duration = duration ?? 0;

            UpdatedAt = DateTime.Now;
        }
    }
}
