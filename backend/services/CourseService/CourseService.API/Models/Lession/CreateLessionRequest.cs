﻿using CourseService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Lession
{
    public class CreateLessionRequest
    {
        [Required]
        [StringLength(80, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        public string CourseId { get; set; } = null!;

        [Required]
        public string SectionId { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(LessionType))]
        public LessionType Type { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must not greater than {1}.")]
        public string? Description { get; set; }
    }
}
