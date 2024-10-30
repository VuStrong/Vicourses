FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CourseService.API/*.csproj ./CourseService.API/
COPY CourseService.Application/*.csproj ./CourseService.Application/
COPY CourseService.Domain/*.csproj ./CourseService.Domain/
COPY EventBus/*.csproj ./EventBus/
COPY CourseService.Infrastructure/*.csproj ./CourseService.Infrastructure/
COPY CourseService.Shared/*.csproj ./CourseService.Shared/
RUN dotnet restore "./CourseService.API/CourseService.API.csproj"
COPY . .
WORKDIR "/src/CourseService.API"
RUN dotnet build "CourseService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourseService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseService.API.dll"]