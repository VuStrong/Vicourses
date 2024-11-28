
# Vicourses Wishlist Service
The Wishlist Service allows students to manage their wishlist, (like add, remove courses and get wishlist).

Each student can only add a maximum 50 of courses to their wishlist.

## Technologies

- .NET Core 8.0
- ASP.NET Core Web API
- MongoDB
- RabbitMQ

## Run this service

1. At the **WishlistService** directory, restore required packages by running:

```shell
  dotnet restore "./WishlistService.API/WishlistService.API.csproj"
```

2. Navigate to **WishlistService.API** directory and provide connection strings in `appsettings.json`

3. Make sure the `public.key` file from [user_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/user_service#run-this-service) is copied to **WishlistService.API** directory

4. Run:
```shell
  dotnet run
```

## Communicate with other microservices
- Consume messages from **course.published**, **course.info.updated** and **course.unpublished** exchange to store course infomation in database.
- Consume messages from **user.enrolled** to remove course from user wishlist after they enrolled.
