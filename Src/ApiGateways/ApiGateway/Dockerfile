#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Src/ApiGateways/ApiGateway/ApiGateway.csproj", "Src/ApiGateways/ApiGateway/"]
COPY ["Src/Services/Common/Common/Common.csproj", "Src/Services/Common/Common/"]
COPY ["Src/BuildingBlocks/EventBus.Core/EventBus.Core.csproj", "Src/BuildingBlocks/EventBus.Core/"]
COPY ["Src/BuildingBlocks/EventBus.RabbitMq/EventBus.RabbitMq.csproj", "Src/BuildingBlocks/EventBus.RabbitMq/"]
RUN dotnet restore "Src/ApiGateways/ApiGateway/ApiGateway.csproj"
COPY . .
WORKDIR "/src/Src/ApiGateways/ApiGateway"
RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]