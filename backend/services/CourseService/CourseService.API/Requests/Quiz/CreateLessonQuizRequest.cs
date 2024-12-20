﻿using CourseService.Application.Dtos.Lesson;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests.Quiz
{
    public class CreateLessonQuizRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        [Length(2, 5, ErrorMessage = "{0} length must be between {1} and {2}")]
        public List<CreateUpdateQuizAnswerRequest> Answers { get; set; } = [];

        public CreateLessonQuizDto ToCreateLessonQuizDto(string userId)
        {
            List<CreateUpdateLessonQuizAnswerDto> answersDto = [];

            foreach (var answers in Answers)
            {
                answersDto.Add(new CreateUpdateLessonQuizAnswerDto(
                    answers.Title,
                    answers.IsCorrect,
                    answers.Explanation
                ));
            }

            return new CreateLessonQuizDto(userId, Title)
            {
                Answers = answersDto
            };
        }
    }

    public class CreateUpdateQuizAnswerRequest
    {
        [Required]
        [StringLength(200, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Title { get; set; } = null!;

        public bool IsCorrect { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string? Explanation { get; set; }
    }
}
