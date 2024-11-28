
# Vicourses Course Service

The Course Service is responsible for course management
- Manage courses: create, edit information.
- Manage course's resourses (sections, lessons, quizzes, comments, ...)
- Retrive courses: filtering, searching
- This service is also responsible for enrolling student in a course


## Technologies

- .NET Core 8.0
- ASP.NET Core Web API
- MongoDB
- RabbitMQ


## Installation

1. At the **CourseService** directory, restore required packages by running:

```shell
  dotnet restore "./CourseService.API/CourseService.API.csproj"
```

2. Navigate to **CourseService.API** directory and provide connection strings in `appsettings.json`

3. Make sure the `public.key` file from [user_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/user_service#run-this-service) is copied to **CourseService.API** directory

4. Run:
```shell
  dotnet run
```

Launch http://localhost:5182/swagger in your browser to view the Swagger UI.
## Communicate with other microservices
- Consume messages from **user.created** and **user.info.updated** exchange to store user infomation in database.
- Consume messages from **course.rating.updated** exchange to update course rating.
- Send messages to **process_video** queue to request video processing for courses and lessons. And consumes messages from **video-processing.completed** and **video-processing.failed** to update video status after being processed.
- After the course is Published, send message to **course.published** exchange. Other microservices will consume this message to store course information in it own database.