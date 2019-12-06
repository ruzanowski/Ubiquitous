# UBIQUITOUS. Real-Time Notification Manager.
- [x] _Open Source_
- [x] _Distributed Event-Based System with Domain Driven Design approach_
- [x] _Communication served by RabbitMQ & SignalR_


- [1. Functionalities](#-1-functionalities)
    - [1.1 Purpose](#11-purpose)
- [2. Server Side Architecture](#1goal-functionalities)
    - [2.1 Technologies And Tools](#21-technologies-and-tools)
    - [2.2 Services](#-22-services)
    - [2.3 Cross-Cutting Concerns](#23-cross-cutting-concerns)
    - [2.4 API - Front communication](#24-api---front-communication)
    - [2.5 Security](#25-security)
- [3. Client Side Overview](#-3-client-side-overview)
    - [3.1 Client Side technologies and tools](#31-client-side-technologies-and-tools)
- [4. Notification Management](#4-notification-management)
    - [4.1 Handling User Subscriptions](#41-handling-user-subscriptions)
- [5. RoadMap](#5-roadmap)
- [6. Contributing](#6-contributing)
- [7. Author](#7-author)
- [8. Inspirations & Thanks](#8-inspirations--thanks)
- [9. License](#9-license)


| *Master*  | *Develop* |
| --------- | --------- |
| [![Master](https://gitlab.com/Ruzanowski/ubiquitous/badges/master/build.svg)](https://gitlab.com/Ruzanowski/ubiquitous/badges/master/build.svg) | [![Develop](https://gitlab.com/Ruzanowski/ubiquitous/badges/develop/build.svg)](https://gitlab.com/Ruzanowski/ubiquitous/badges/develop/build.svg)


# 1. Introduction

## 1.1. Functionalities
- ***Notifications*** :bell:
    - Channels
        * [x] signalR (WebSocket)
        * [x] push notification (Toastr)
        * [ ] e-mail
    - Types
        * [x] product published
        * [x] product added
        * [x] product properties changed
        * [x] user connected
        * [x] user disconnected
        * [ ] user signed up
        * [ ] user signed in
    - Operations
        * [x] auto-confirmation
        * [x] hide notification
        * [x] remove notification
    - States
        * [x] trivial
        * [x] normal
        * [x] important
    - Persistency
        * [x] notifications with states are persisted in PestgreSQL
        * [x] notifications from last 24 hours are sent during logging in 
    - Accessibility
        * [x] visible and operable from website with all entire specification
- ***Identity & Authorization*** :bell:
    - Identity :card_index:
        - Operations
            * [x] log in
            * [x] log out
            * [x] change password
        - Properties
            - id
            - email
            - password
            - role
            - nickname
            - claims
            - access token
            - refresh token
    - Authorization :old_key:
        - Jwt Token Operations
            * [x] create
            * [x] revoke 
            * [x] refresh
    * [x] each user has identificator with 
- ***Admin Management (next versions)*** 
    - Operations 
        * [ ] manage products and its details thru product service API
        * [ ] manage users subscriptions
        * [ ] manage subscribers notifications

## 1.2 Purpose
- **prime goal** is/was an education and giving my best around programming topics like 
     - .NET Core
     - DDD
     - Microservices
     - Docker
     - Angular
     - ...
     - and so much more
- Secondly **most of repositories** I have visited were
    - Relatively small, no possibility to run into issues of performance
    - Easy concerns or well known domain (orders, eShop)
    - Unfinished
    
## 1.3 Install

1.3.0. Prerequisites

    - Docker
    - Docker composer
    
1.3.1. Run 
```cmd
docker-compose -f docker-compose-infrastructure.yml up
```
1.3.2. Enter localhost:5450 (PGAdmin) and run
```postgresql
CREATE DATABASE product-service;
CREATE DATABASE smartstore-adapter;
CREATE DATABASE fetch-service;
CREATE DATABASE notification-service;
CREATE DATABASE subscription-service;

```

1.3.3. Replace value from file '.env' and row 'RELATIVEPATH' to your relative path indicating folder containing folders with appsettings or adjust them to your preferenes.

1.3.4. Run 

```cmd
docker-compose -f docker-compose-services.yml up
```

1.3.5. You can manage all containers from portainer dashboard.

<p align="center">
   <img alt="Portainer dashboard" src="img/Portainer.png" />
</p>
    
## 2. Server Side Architecture
Whole solution is designed and broken down to
   - **Frontend** is (being) implemented  in **Angular 7** with **Angular Material** as UI component infrastructure and Material Design components.
   - **Backend** is (being) written in **.NET Core 2.2**(current stable version). 

Down below, a services dependency diagram. See to #3.1 for listed used technologies, tools and their use.
    
<p align="center">
   <img alt="Ubiquitous Service Architecture" src="img/ubiquitous-architecture.png" />
</p>

### 2.1 Technologies And Patterns
- ***EF Core 2.2*** *(ORM)*
- ***AutoMapper*** *(Objects mappings)*
- ***MediatR*** *(CQRS pattern dispatch)*
- ***Polly*** *(Resiliency policies)*
- ***RabbitMQ*** *(Service-service communication)*
- ***Consul*** *(Service discovery, keep alive)*
- ***Fabio*** *(Load balancer)*
- ***Docker*** *(Containers environment)*
- ***Serilog*** *(Logging)*
- ***PostgreSql*** *(Database)*
- ***Gitlab DevOps*** *(CI)*
- ***SignalR*** *(Asynchronous communication, Notifications)*
- ***Redis*** Distributed caching & SignalR backplane
- ***Ocelot*** API Gateway
- ***Jaeger*** [todo] Tracing

### 2.2 Services
-------
**Services**
- ***SmartStore Adapter*** Wholesale, source of data
- ***Fetch Service*** Fetches data from wholesales(many) and pushes newest items on bus
- ***Product Service*** Main domain aggregate service, handles products and its business logic
- ***Notification Service*** Handles notifications and channels it by WebSocket
- ***Identity Service*** Handles identity of user and managed Jwt tokens
- ***Subscription Service*** Handles subscriptions of users & preferences for notifications types, channels
-------
### 2.3 Cross-Cutting Concerns

**Modules**
- ***IntegrationEvent Log*** 
    - Persistent Module with shared Unit Of Work logic for integration events for UDP-like communication
- ***EventBus RabbitMQ*** 
    - RabbitMQ asynchronous queue shared logic of subscription and publishing
- ***Common*** 
    - Shared logic
        - Tracing (Jaeger)
        - Logging (Serilog)
        - Service Discovery (Consul)
        - Load Balancing (Fabio)
        - Resiliency (Polly)
        - Pagination
        
### 2.4 API - Front communication

_To be determined_
  
### 2.5 Security

_To be determined_
        
## 3. Client Side Overview

<p align="center">
   <img alt="Ubiquitous front-end progress" src="img/ubiquitous-current-state.png" />
</p>

### 3.1 Client Side technologies and tools
- ***Angular 7***
- ***Angular Material*** 
- ***RxJS***
- ***ASPNET SignalR***

## 4. Notification Management

_To be determined_


### 4.1 Handling User Subscriptions

_To be determined_

## 5. RoadMap

| *Task*  | *Priority* |*Status* | *Date* |
| ------- | ---------- | ------  | ------ |
|SmartStore Adapter|-----|Completed|07.2019|
|Fetch Service|-----|Completed|07.2019|
|ProductsGenerator Service|-----|Completed|07.2019|
|Report Service(Caracan Liquid Project) -- Deprecated|-----|Completed|08.2019|
|Dockerization|-----|Completed|09.2019
|Gitlab CI Pipelines|-----|Completed|09.2019
|Product Service - Major Features|Critical|Completed|10.2019|
|Dashboard Web-Side| Critical|Completed|10.2019|
|Notifications SignalR & Basic Features|Critical|Completed|10.2019|
|Notifications Management(confirm, hide, remove, mute)|Critical|Completed|11.2019|
|Identity Service|Critical|Completed|11.2019|
|Jwt Authorization|Critical|Completed|11.2019|
|API Gateway|High|Completed|11.2019|
|Subscriptions Service|High|Completed|12.2019|
|Notifications new channels (push)|Medium|Completed|12.2019|
|Preferences FE-BE Integration|High|In Progress||
|Dashboard FE-BE data sourcing|High|In Progress||
|Tracing (Jaeger)|Medium|||
|Admin Web-Side Panel (products)|Medium|||
|Admin Web-Side Panel (subscribers, users)|Medium|||
|Notification by E-mail| Medium||
|Notifications new channels (email)|Medium-Low|||
|Migration to .NET Core 3.0 |Low|||
|Security (HTTPS)|Low|||

## 6. Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## 7. Author

Sebastian Rużanowski

LinkedIn [https://www.linkedin.com/in/sebastian-ruzanowski](https://www.linkedin.com/in/sebastian-ruzanowski)

GitLab: [https://gitlab.com/ruzanowski](https://gitlab.com/ruzanowski)

## 8. Inspirations & Thanks

- Repositories
    - [DevMentors](https://github.com/devmentors)
    - [Dotnet](https://github.com/dotnet-architecture/eShopOnContainers)
    - [Modular Monolith with DDD](https://github.com/kgrzybek/modular-monolith-with-ddd)
    - [ASC Lab](https://github.com/asc-lab/dotnetcore-microservices-poc) 
- Sites & Blogs
    - [Microservices.io](https://microservices.io/)
    - [Creative Tim](https://www.creative-tim.com/)
    
## 9. License
[MIT](https://choosealicense.com/licenses/mit/)