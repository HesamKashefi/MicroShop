# MicroShop E-Shop

This is a sample project for Microservices Architecture that I've developed just to learn by practice!

Since I've been learning by looking at the Microsoft's sample, [EShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers), You'll see many similarities here to that project!

### Some of the technologies, patterns and tools used in this project
- \#Docker
- \#ASP.NET Core
- \#SQL Server
- \#MongoDB
- \#Redis
- \#RabbitMQ
- \#gRPC
- \#OpenIddict
- \#Datalust_Seq
- \#NLOG
- \#DDD
- \#EventDrivenArchitecture
- \#CQRS
- \#YARP
- \#Angular

## Architecture

### Addresses:

| Service   |     Address     |
|----------|:-------------:|
|  View  |  https://+:8000 |
|  Admin  |  http://+:7011 |
|  APIGateway  |  https://+:8001 |
|  Identity.Api  |  https://+:8002 |
|  Catalog.Api  |  https://+:8003 |
|  Orders.Api  |  https://+:8004 |
|  Identity.Api  |  https://+:8005 |
|  HealthChecks  |  https://+:8010/hc-ui |
|  Seq  |  http://+:5340 |


### View
https://+:8000

View is an AspNetCore UI

### APIGateway
https://+:8001

The APIGateway implemented using Yarp.ReverseProxy

### Identity.Api
https://+:8002

This service is responsible for Authentication.

I've used OpenIddict-Core library to create an openid connect server and SqlServer as the database.


### Catalog.Api
https://+:8003

Maintains a list of products. 

Built using CQRS pattern.

MongoDb is used as the database.


### Cart.Api
https://+:8004

Cart/Basket information.

Redist is used as the database.



### Seq
http://+:5340

Datalust Seq is used to monitor application logs


### HealthChecks
https://+:8010/hc-ui

Use to monitor the health of the services