#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#debug in visual studio
#note: visual studio will mount the directory automattically for debug, so we don't need to copy source files
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS debug
#install nodejs
RUN apt-get update
RUN apt-get -y install curl gnupg
RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get -y install nodejs
#install our npm dependencies
WORKDIR /app/ClientApp
#COPY Src/View/MicroShop.Admin/ClientApp/package.json .
#COPY Src/View/MicroShop.Admin/ClientApp/package-lock.json .
#RUN npm install
#RUN npm -v
EXPOSE 80

From node:20-alpine AS node-build
WORKDIR /app
COPY Src/View/MicroShop.Admin/ClientApp/package.json .
COPY Src/View/MicroShop.Admin/ClientApp/package-lock.json .
RUN npm install
COPY Src/View/MicroShop.Admin/ClientApp .
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["MicroShop.sln", "MicroShop.sln"]

COPY ["Src/View/MicroShop.View/MicroShop.View.csproj", "Src/View/MicroShop.View/"]
COPY ["Src/View/MicroShop.Admin/MicroShop.Admin.csproj", "Src/View/MicroShop.Admin/"]
COPY ["Src/ApiGateways/ApiGateway/ApiGateway.csproj", "Src/ApiGateways/ApiGateway/"]
COPY ["Src/View/MicroShop.HealthChecksUI/MicroShop.HealthChecksUI.csproj", "Src/View/MicroShop.HealthChecksUI/"]
COPY ["Src/Services/Identity/Identity.Api/Identity.Api.csproj", "Src/Services/Identity/Identity.Api/"]
COPY ["Src/Services/Cart/Cart.Api/Cart.Api.csproj", "Src/Services/Cart/Cart.Api/"]
COPY ["Src/Services/Orders/Orders.SignalR/Orders.SignalR.csproj", "Src/Services/Orders/Orders.SignalR/"]
COPY ["Src/Services/Orders/Orders.Api/Orders.Api.csproj", "Src/Services/Orders/Orders.Api/"]
COPY ["Src/Services/Orders/Orders.Application/Orders.Application.csproj", "Src/Services/Orders/Orders.Application/"]
COPY ["Src/Services/Orders/Orders.Domain/Orders.Domain.csproj", "Src/Services/Orders/Orders.Domain/"]
COPY ["Src/Services/Orders/Orders.Persistence/Orders.Persistence.csproj", "Src/Services/Orders/Orders.Persistence/"]
COPY ["Src/Services/Orders/Tests.Orders/Tests.Orders.csproj", "Src/Services/Orders/Tests.Orders/"]
COPY ["Src/Services/Payments/Payments.Api/Payments.Api.csproj", "Src/Services/Payments/Payments.Api/"]
COPY ["Src/Services/Catalog/Catalog.Api/Catalog.Api.csproj", "Src/Services/Catalog/Catalog.Api/"]
COPY ["Src/Services/Catalog/Catalog.Application/Catalog.Application.csproj", "Src/Services/Catalog/Catalog.Application/"]
COPY ["Src/Services/Catalog/Catalog.Domain/Catalog.Domain.csproj", "Src/Services/Catalog/Catalog.Domain/"]
COPY ["Src/Services/Common/Common/Common.csproj", "Src/Services/Common/Common/"]
COPY ["Src/BuildingBlocks/EventBus.RabbitMq/EventBus.RabbitMq.csproj", "Src/BuildingBlocks/EventBus.RabbitMq/"]
COPY ["Src/BuildingBlocks/EventBus.Core/EventBus.Core.csproj", "Src/BuildingBlocks/EventBus.Core/"]
COPY ["Src/BuildingBlocks/EventLog/EventLog.csproj", "Src/BuildingBlocks/EventLog/"]

COPY ["NuGet.config", "NuGet.config"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]

COPY ["docker-compose.dcproj", "docker-compose.dcproj"]

RUN dotnet restore "MicroShop.sln"
COPY . .
COPY --from=node-build /app Src/View/MicroShop.Admin/wwwroot/
WORKDIR "/src/Src/View/MicroShop.Admin"
RUN dotnet build "MicroShop.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroShop.Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 81
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroShop.Admin.dll"]