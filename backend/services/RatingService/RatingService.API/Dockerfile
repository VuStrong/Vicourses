FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY RatingService.API/*.csproj ./RatingService.API/
COPY EventBus/*.csproj ./EventBus/
RUN dotnet restore "./RatingService.API/RatingService.API.csproj"
COPY . .
WORKDIR "/src/RatingService.API"
RUN dotnet build "RatingService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RatingService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RatingService.API.dll"]