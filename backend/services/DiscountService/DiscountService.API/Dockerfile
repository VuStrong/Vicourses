FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY DiscountService.API/*.csproj ./DiscountService.API/
COPY EventBus/*.csproj ./EventBus/
RUN dotnet restore "./DiscountService.API/DiscountService.API.csproj"
COPY . .
WORKDIR "/src/DiscountService.API"
RUN dotnet build "DiscountService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscountService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscountService.API.dll"]