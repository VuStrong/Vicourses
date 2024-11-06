FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WishlistService.API/*.csproj ./WishlistService.API/
COPY EventBus/*.csproj ./EventBus/
RUN dotnet restore "./WishlistService.API/WishlistService.API.csproj"
COPY . .
WORKDIR "/src/WishlistService.API"
RUN dotnet build "WishlistService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WishlistService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WishlistService.API.dll"]