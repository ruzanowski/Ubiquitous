# UBIQUITOUS. Real-Time Notification Manager.
- [x] _Open Source_
- [x] _Distributed Event-Based System with Domain Driven Design approach_
- [x] _Communication served by RabbitMQ & SignalR_


| *Master*  | *Develop* |
| --------- | --------- |
| [![Master](https://gitlab.com/Ruzanowski/ubiquitous/badges/master/build.svg)](https://gitlab.com/Ruzanowski/ubiquitous/badges/master/build.svg) | [![Develop](https://gitlab.com/Ruzanowski/ubiquitous/badges/develop/build.svg)](https://gitlab.com/Ruzanowski/ubiquitous/badges/develop/build.svg)

[1. Goal functionalities](#1goal-functionalities)
[2. Purpose](#2purpose)



## 1. Goal functionalities
----
-  ***Notifications management*** 
    - Channels
        * [x] SignalR (WebSocket)
        * [ ] Push notification (Toastr)
        * [ ] E-mail
    - Types
        * [x] product published
        * [x] product added
        * [x] product properties changed
    - Operations
        * [ ] auto-confirm received notification
        * [ ] hide notification
        * [ ] remove notification
        * [ ] silence type of notification
-  ***Notifications persistency*** 
    - each notification and subscribers state are persisted in PostgreSQL
    - welcome notifications are sent from last 24hrs for first log-in
* [ ] ***admin management (next versions)*** 
    - Operations 
        * [ ] manage products and its details thru product service API
        * [ ] manage users subscriptions
        * [ ] manage subscribers notifications

## 1.1 Purpose
- **prime goal** is/was an education and giving my best around programming topics like 
     - .NET Core
     - Microservices
     - Docker
     - DDD
     - Angular
     - ...
     - and so much more
- Secondly **most of repositories** I have visited were
    - Relatively small, no possibility to run into issues of performance
    - Easy concerns or well known domain (orders, eShop)
    - Unfinished
    
## 2. Server Side Architecture
Whole solution is designed and broken down to
   - **Frontend** is (being) implemented  in **Angular 7** with **Angular Material** as UI component infrastructure and Material Design components.
   - **Backend** is (being) written in **.NET Core 2.2**(current stable version). 

Down below, a services dependency diagram. See to #3.1 for listed used technologies, tools and their use.
    
<p align="center">
   <img alt="Ubiquitous Service Architecture" src="img/ubiquitous-architecture.png" />
</p>

## 2.1 Server Side Technologies And Tools
- ***EF Core 2.2*** *(ORM)*
- ***RabbitMQ*** *(Service-service communication)*
- ***Consul*** *(Service discovery, keep alive)*
- ***Fabio*** *(Load balancer)*
- ***MediatR*** *(CQRS pattern dispatch)*
- ***Docker*** *(Containers environment)*
- ***Polly*** *(Resiliency policies)*
- ***Serilog*** *(Logging)*
- ***AutoMapper*** *(Objects mappings)*
- ***PostgreSql*** *(Database)*
- ***Gitlab DevOps*** *(CI)*
- ***SignalR*** *(Asynchronous communication, Notifications)*
- ***Redis*** Distributed caching & SignalR backplane

- ***Ocelot*** [todo] ApiGW
- ***Jaeger*** [todo] tracing
- ***Grafana*** [todo] metrics
- ***Prometheus*** [todo] metrics infrastructure

## 2.2 Services
-------
**Services**
- ***SmartStore Adapter*** Wholesale, source of data
- ***Fetch Service*** Fetches data from wholesales(many) and pushes newest items on bus
- ***Product Service*** Main domain aggregate service, handles products and its business logic
- ***Report Service*** Handles reports and its generation thanks to [Caracan](https://github.com/caracan-team)
- ***Notification Service*** Handles notifications and channels it by WebSocket
-------
## 2.3 Cross-Cutting Concerns

**Modules**
- ***IntegrationEvent Log*** Shared integration events dbContext for each project
- ***EventBus RabbitMQ*** 
    - RabbitMQ asynchronous queue shared logic
- ***Common*** 
    - Shared logic
        - Tracing (Jaeger)
        - Logging (Serilog)
        - Service Discovery (Consul)
        - Load Balancing (Fabio)
        - Resiliency (Polly)
        - Pagination
## 3. Client Side Overview
-------

<p align="center">
   <img alt="Ubiquitous front-end progress" src="img/ubiquitous-current-state.png" />
</p>

## 4.1 Client Side technologies and tools
- ***Angular 7***
- ***Angular Material*** 
- ***RxJS***
- ***ASPNET SignalR***




#xxxxxxxxxxxxRoadMap

| *Task*  | *Priority* |*Status* | *Date* |
| ------- | ---------- | ------  | ------ |
|SmartStore Adapter|-----|Completed|07.2019|
|Fetch Service|-----|Completed|07.2019|
|ProductsGenerator Service|-----|Completed|07.2019|
|Report Service(Caracan Liquid Project)|-----|Completed|08.2019|
|Dockerization|-----|Completed|09.2019
|Gitlab CI Pipelines|-----|Completed|09.2019
|Product Service - Major Features|Critical|Completed|10.2019|
|Dashboard Web-Side| Critical|Completed|10.2019|
|Notifications SignalR & Basic Features|Critical|Completed|10.2019|
|Notifications Management(confirm, hide, remove, mute)|Critical|In Progress||
|Identity Service|Critical|||
|Security (HTTPS)|High|||
|Notifications new channels (push, e-mail)|Medium||
|Admin Web-Side Panel (products)|Medium|||
|Admin Web-Side Panel (subscribers, users)|Medium|||
|Notification by E-mail| Medium||
|Migration to .NET Core 3.0 |Low|||

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.


## Inspired by (Repos worth to see)
- [DevMentors](https://github.com/devmentors) and their great tutorial about .NET Core with top-class explanation
- [Dotnet](https://github.com/dotnet-architecture/eShopOnContainers) obviously, #1 architectural hats-off project
- [ASC-Lab](https://github.com/asc-lab/dotnetcore-microservices-poc) 

## License
[MIT](https://choosealicense.com/licenses/mit/)
