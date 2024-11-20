
# Vicourses Search Service
The Search Service is integrated with Elasticsearch to deliver full-text search capabilities for published courses.

## Technologies
- .NET Core 8.0
- ASP.NET Core Web API
- Elasticsearch
- RabbitMQ

## Run this service

1. At the **SearchService** directory, restore required packages by running:

```shell
  dotnet restore "./SearchService.API/SearchService.API.csproj"
```

2. Navigate to **SearchService.API** directory and provide connection strings for RabbitMQ and configuration for Elasticsearch in `appsettings.json`

3. Run:
```shell
  dotnet run
```
Launch http://localhost:5054/swagger in your browser to view the Swagger UI.

## Communicate with other microservices
- In order to have data for searching, this service consume messages from **course.published** exchange and index course information to Elasticsearch.
- Consume messages from **course.unpublished** exchange to remove course from Elasticsearch.