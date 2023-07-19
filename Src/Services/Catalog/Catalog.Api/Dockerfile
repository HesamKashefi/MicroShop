#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Src/Services/Catalog/Catalog.Api/Catalog.Api.csproj", "Src/Services/Catalog/Catalog.Api/"]
COPY ["Src/Services/Common/Common/Common.csproj", "Src/Services/Common/Common/"]
COPY ["Src/BuildingBlocks/EventBus.Core/EventBus.Core.csproj", "Src/BuildingBlocks/EventBus.Core/"]
COPY ["Src/BuildingBlocks/EventBus.RabbitMq/EventBus.RabbitMq.csproj", "Src/BuildingBlocks/EventBus.RabbitMq/"]
COPY ["Src/Services/Catalog/Catalog.Application/Catalog.Application.csproj", "Src/Services/Catalog/Catalog.Application/"]
COPY ["Src/Services/Catalog/Catalog.Domain/Catalog.Domain.csproj", "Src/Services/Catalog/Catalog.Domain/"]
RUN dotnet restore "Src/Services/Catalog/Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Src/Services/Catalog/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]