
# Vicourses Payment Service

- The Payment Service is responsible for payment processing. It uses Paypal as payment gateway.
- This service communicate with the [DiscountService](https://github.com/VuStrong/Vicourses/tree/main/backend/services/DiscountService) Grpc to check the coupon if applied and get course final price.
- After a payment is completed, store the payment in database and publish the **payment.completed** event.
- Todos: Add refund API

## Technologies
- .NET Core 8.0
- ASP.NET Core Web API
- MySQL with Entity Framework Core 8.0
- RabbitMQ
- Grpc

## Installation

1. At the **PaymentService** directory, restore required packages by running:

```shell
  dotnet restore "./PaymentService.API/PaymentService.API.csproj"
```

2. Navigate to **PaymentService.API** directory and provide connection strings, paypal configurations in `appsettings.json`

3. Create database
```shell
  dotnet ef database update
``` 

4. Make sure the `public.key` file from [user_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/user_service#run-this-service) is copied to **PaymentService.API** directory

5. Run:
```shell
  dotnet run
```

Launch http://localhost:5053/swagger in your browser to view the Swagger UI.