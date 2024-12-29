using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Application.Utils;
using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;
using CourseService.Shared.Paging;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CourseService.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly string _mediaSecret;

        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;
        private readonly IFileUploadTokenValidator _fileUploadTokenValidator;

        public CourseService(
            ICourseRepository courseRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CourseService> logger,
            IFileUploadTokenValidator fileUploadTokenValidator,
            ApplicationConfiguration applicationConfiguration)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
            _fileUploadTokenValidator = fileUploadTokenValidator;
            _mediaSecret = applicationConfiguration.MediaFileSecret;
        }

        public async Task<PagedResult<CourseDto>> GetCoursesAsync(GetCoursesParamsDto? paramsDto = null)
        {
            int skip = (paramsDto == null || paramsDto.Skip < 0) ? 0 : paramsDto.Skip;
            int limit = (paramsDto == null || paramsDto.Limit <= 0) ? 10 : paramsDto.Limit;

            var results = await _courseRepository.FindManyAsync(
                skip, limit, 
                sort: paramsDto?.Sort,
                searchKeyword: paramsDto?.SearchKeyword,
                categoryId: paramsDto?.CategoryId,
                subCategoryId: paramsDto?.SubCategoryId,
                isPaid: paramsDto?.Free != null ? !paramsDto.Free : null,
                level: paramsDto?.Level,
                minimumRating: paramsDto?.MinimumRating,
                status: paramsDto?.Status ?? CourseStatus.Published,
                tag: paramsDto?.Tag);

            return _mapper.Map<PagedResult<CourseDto>>(results);
        }

        public async Task<PagedResult<CourseDto>> GetCoursesByUserIdAsync(string userId, int skip, int limit, string? searchKeyword = null,
            CourseStatus? status = null)
        {
            skip = skip < 0 ? 0 : skip;
            limit = limit <= 0 ? 10 : limit;

            var results = await _courseRepository.FindManyByUserIdAsync(userId, skip, limit, searchKeyword, status);

            return _mapper.Map<PagedResult<CourseDto>>(results);
        }

        public async Task<CourseDetailDto> GetCourseDetailByIdAsync(string courseId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            var courseDto = _mapper.Map<CourseDetailDto>(course);

            if (course.PreviewVideo != null && course.PreviewVideo.Status == VideoStatus.Processed)
            {
                var token = JwtHelper.GenerateJWT(_mediaSecret, (descriptor) => {
                    descriptor.Expires = DateTime.Now.AddHours(2);
                    descriptor.Subject = new ClaimsIdentity(new List<Claim> {
                        new("manifestFileId", course.PreviewVideo.ManifestFileId ?? ""),
                        new("fileId", course.PreviewVideo.FileId)
                    });
                });

                courseDto.PreviewVideo!.Token = token;
            }

            return courseDto;
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto data)
        {
            var category = await _categoryRepository.FindOneAsync(
                c => c.Id == data.CategoryId && c.ParentId == null
            );
            if (category == null)
            {
                throw new CategoryNotFoundException(data.CategoryId);
            }

            var subCategory = await _categoryRepository.FindOneAsync(
                c => c.Id == data.SubCategoryId && c.ParentId == category.Id    
            );
            if (subCategory == null)
            {
                throw new CategoryNotFoundException(data.SubCategoryId);
            }

            var user = await _userRepository.FindOneAsync(data.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(data.UserId);
            }

            var course = Course.Create(data.Title, data.Description, category, subCategory, user);

            await _courseRepository.CreateAsync(course);

            _logger.LogInformation("A course was created with Id = {Id}", course.Id);

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> UpdateCourseAsync(string courseId, UpdateCourseDto data, string ownerId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            if (course.User.Id != ownerId)
            {
                throw new ForbiddenException($"Forbidden resourse");
            }

            Category? categoryToUpdate = null;
            if (data.CategoryId != null && data.CategoryId != course.Category.Id)
            {
                categoryToUpdate = await _categoryRepository.FindOneAsync(
                    c => c.Id == data.CategoryId && c.ParentId == null
                ) ?? throw new CategoryNotFoundException(data.CategoryId);
            }

            Category? subCategoryToUpdate = null;
            if (data.SubCategoryId != null && (data.SubCategoryId != course.SubCategory.Id || categoryToUpdate != null))
            {
                var categoryIdToCheck = categoryToUpdate?.Id ?? course.Category.Id;

                subCategoryToUpdate = await _categoryRepository.FindOneAsync(
                    c => c.Id == data.SubCategoryId && c.ParentId == categoryIdToCheck
                ) ?? throw new CategoryNotFoundException(data.SubCategoryId);
            }

            ImageFile? thumbnail = null;
            if (data.ThumbnailToken != null)
            {
                var uploadFileDto = _fileUploadTokenValidator.ValidateFileUploadToken(data.ThumbnailToken, ownerId);

                thumbnail = ImageFile.Create(uploadFileDto.FileId, uploadFileDto.Url);
            }

            VideoFile? previewVideo = null;
            if (data.PreviewVideoToken != null)
            {
                var uploadFileDto = _fileUploadTokenValidator.ValidateFileUploadToken(data.PreviewVideoToken, ownerId);

                previewVideo = VideoFile.Create(uploadFileDto.FileId, uploadFileDto.Url, uploadFileDto.OriginalFileName);
            }

            course.UpdateInfoIgnoreNull(data.Title, data.Description, data.Tags, data.Requirements, data.TargetStudents,
                data.LearnedContents, data.Price, data.Locale, categoryToUpdate, subCategoryToUpdate, data.Level);

            if (data.Status != null)
                course.SetStatus(data.Status.Value);
            
            if (thumbnail != null)
                course.UpdateThumbnail(thumbnail);

            if (previewVideo != null)
                course.UpdatePreviewVideo(previewVideo);

            await _courseRepository.UpdateAsync(course);

            return _mapper.Map<CourseDto>(course);
        }

        public async Task DeleteCourseAsync(string courseId, string ownerId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);
            
            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            if (course.User.Id != ownerId)
            {
                throw new ForbiddenException($"Forbidden resourse");
            }

            course.SetDeleted();

            await _courseRepository.UpdateAsync(course);
        }

        public async Task ApproveCourseAsync(string courseId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            course.Approve();

            await _courseRepository.UpdateAsync(course);
        }

        public async Task CancelCourseApprovalAsync(string courseId, List<string> reasons)
        {
            var course = await _courseRepository.FindOneAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            course.CancelApproval(reasons);

            await _courseRepository.UpdateAsync(course);
        }

        public async Task<CourseCheckResultDto> CheckCourseAsync(string courseId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            var result = new CourseCheckResultDto()
            {
                IsValid = true,
            };

            if (course.Description == null || course.Description.Length < 100)
            {
                result.IsValid = false;
                result.MissingRequirements.Add("Your course description must be at least 100 characters.");
            }
            if (course.Tags.Count < 2)
            {
                result.IsValid = false;
                result.MissingRequirements.Add("Your course must have at least 2 tags.");
            }
            if (course.Requirements.Count == 0)
            {
                result.IsValid = false;
                result.MissingRequirements.Add("You must clearly state any course requirements or prerequisites.");
            }
            if (course.TargetStudents.Count == 0)
            {
                result.IsValid = false;
                result.MissingRequirements.Add("You must clearly state the target student of this course.");
            }
            if (course.LearnedContents.Count < 4)
            {
                result.IsValid = false;
                result.MissingRequirements.Add("You must state at least 4 learned contents of this course.");
            }
            if (course.Thumbnail == null)
            {
                result.IsValid = false;
                result.MissingRequirements.Add("You must upload your course thumbnail.");
            }
            if (course.Metrics.LessonsCount < 5)
            {
                result.IsValid = false;
                result.MissingRequirements.Add("Your course must have at least 5 lessons");
            }

            return result;
        }
    }
}
