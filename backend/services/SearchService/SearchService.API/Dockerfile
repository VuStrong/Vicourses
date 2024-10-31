FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SearchService.API/*.csproj ./SearchService.API/
COPY EventBus/*.csproj ./EventBus/
RUN dotnet restore "./SearchService.API/SearchService.API.csproj"
COPY . .
WORKDIR "/src/SearchService.API"
RUN dotnet build "SearchService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SearchService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchService.API.dll"]