﻿using CourseService.Application.Dtos.Lesson;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests.Quiz
{
    public class UpdateLessonQuizRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        [Length(2, 5, ErrorMessage = "{0} length must be between {1} and {2}")]
        public List<CreateUpdateQuizAnswerRequest> Answers { get; set; } = [];

        public UpdateLessonQuizDto ToUpdateLessonQuizDto(string userId, int number)
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

            return new UpdateLessonQuizDto
            {
                UserId = userId,
                Number = number,
                Title = Title,
                Answers = answersDto
            };
        }
    }
}
