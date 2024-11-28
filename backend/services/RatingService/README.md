
# Vicourses Rating Service

The Rating Service is responsible for rating courses

- Student can create a rating after they enrolled in a course. Each student can only rate a course once.
- Instructors can respond to their course's ratings.
## Technologies
- .NET Core 8.0
- ASP.NET Core Web API
- MySQL with Entity Framework Core 8.0
- RabbitMQ
## Run this service

1. At the **RatingService** directory, restore required packages by running:

```shell
  dotnet restore "./RatingService.API/RatingService.API.csproj"
```

2. Navigate to **RatingService.API** directory and provide connection strings in `appsettings.json`

3. Create database
```shell
  dotnet ef database update
``` 

4. Make sure the `public.key` file from [user_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/user_service#run-this-service) is copied to **RatingService.API** directory

5. Run:
```shell
  dotnet run
```
Launch http://localhost:5078/swagger in your browser to view the Swagger UI.
    
## Communicate with other microservices
- Consume messages from **user.created** and **user.info.updated** exchange to store user infomation in database.
- Consume messages from **course.published** and **course.unpublished** exchange to store course infomation in database.
- Consume messages from **user.enrolled** in order to know if student is enrolled a course or not.
- Publish **course.rating.updated** message after a course's average rating is updated.