#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroShop.sln", "MicroShop.sln"]

COPY ["Src/View/MicroShop.View/MicroShop.View.csproj", "Src/View/MicroShop.View/"]
COPY ["Src/View/MicroShop.Admin/MicroShop.Admin.csproj", "Src/View/MicroShop.Admin/"]
COPY ["Src/ApiGateways/ApiGateway/ApiGateway.csproj", "Src/ApiGateways/ApiGateway/"]
COPY ["Src/View/MicroShop.HealthChecksUI/MicroShop.HealthChecksUI.csproj", "Src/View/MicroShop.HealthChecksUI/"]
COPY ["Src/Services/Identity/Identity.Api/Identity.Api.csproj", "Src/Services/Identity/Identity.Api/"]
COPY ["Src/Services/Cart/Cart.Api/Cart.Api.csproj", "Src/Services/Cart/Cart.Api/"]
COPY ["Src/Services/Orders/Orders.Api/Orders.Api.csproj", "Src/Services/Orders/Orders.Api/"]
COPY ["Src/Services/Catalog/Catalog.Api/Catalog.Api.csproj", "Src/Services/Catalog/Catalog.Api/"]
COPY ["Src/Services/Catalog/Catalog.Application/Catalog.Application.csproj", "Src/Services/Catalog/Catalog.Application/"]
COPY ["Src/Services/Catalog/Catalog.Domain/Catalog.Domain.csproj", "Src/Services/Catalog/Catalog.Domain/"]
COPY ["Src/Services/Common/Common/Common.csproj", "Src/Services/Common/Common/"]
COPY ["Src/BuildingBlocks/EventBus.RabbitMq/EventBus.RabbitMq.csproj", "Src/BuildingBlocks/EventBus.RabbitMq/"]
COPY ["Src/BuildingBlocks/EventBus.Core/EventBus.Core.csproj", "Src/BuildingBlocks/EventBus.Core/"]

COPY ["NuGet.config", "NuGet.config"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]

RUN dotnet restore "MicroShop.sln"
COPY . .
WORKDIR "/src/Src/Services/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]