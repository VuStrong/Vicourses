
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

2. Make sure **WishlistDB.Uri** in `appsettings.json` point to a local MongoDB Server.

3. Make sure the `public.key` file from [auth_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/auth_service#run-this-service) is copied to **WishlistService.API** directory

4. Navigate to **WishlistService.API** directory and run:
```shell
  dotnet run
```

## Communicate with other microservices
- Consume messages from **course.published**, **course.info.updated** and **course.unpublished** exchange to store course infomation in database.
- Consume messages from **user.enrolled** to remove course from user wishlist after they enrolled.
