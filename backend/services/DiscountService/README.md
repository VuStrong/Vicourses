
# Vicourses Discount Service

The Discount Service enables the instructors to create coupons for their courses. Allow students to redeem coupons (discount codes) during checkout and get discounts on their purchases.

Exposing Grpc Services for other microservices to check coupon, get course price.

## Technologies
- .NET Core 8.0
- ASP.NET Core Web API
- MySQL with Entity Framework Core 8.0
- RabbitMQ
- Redis
- Grpc


## Installation

1. At the **DiscountService** directory, restore required packages by running:

```shell
  dotnet restore "./DiscountService.API/DiscountService.API.csproj"
```

2. Navigate to **DiscountService.API** directory and provide connection strings in `appsettings.json`

3. Create database
```shell
  dotnet ef database update
``` 

4. Make sure the `public.key` file from [user_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/user_service#run-this-service) is copied to **DiscountService.API** directory

5. Run:
```shell
  dotnet run --launch-profile https
```

Launch http://localhost:5161/swagger in your browser to view the Swagger UI.
    
## Running Functional Tests
Navigate to **DiscountService.FunctionalTests** directory

**NOTE**: Functional tests in this project using test containers and require that Docker be running.

```bash
  dotnet test
```

