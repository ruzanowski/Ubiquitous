#UBIQUITOUS. Real-Time Notification Manager.
- _Educational project_
- _Built in .NET Core 2.2 with Angular._
- _Domain Driven Design_

# Build status
-------
- ***Master***:
[![Master](https://gitlab.com/Ruzanowski/ubiquitous/badges/master/build.svg)](https://gitlab.com/Ruzanowski/ubiquitous/badges/master/build.svg)
</br>
- ***Develop***:
[![Develop](https://gitlab.com/Ruzanowski/ubiquitous/badges/develop/build.svg)](https://gitlab.com/Ruzanowski/ubiquitous/badges/develop/build.svg)

# About project
Ubiquitous is an open-source Distributed .NET Core solution for:
- ***product management,*** user has ability to manage products he/she wants to be informed real-time (WebSocket or pulling)
- ***subscription & notifications management,*** user can specify what products or groups of products he/she is interested to be notified
- ***event processing and analysis,*** internal events are being processed and pushed to end user with respective generated event

# Reason behind the project
- ***prime goal*** is education and giving my best around programming topics like DDD, high-performance, distributed systems 
- Most of repositories I have visited are:
    - Relatively small, hence no possibility to run into issues of performance
    - Easy or well known domain (Orders, eShop)
    - Unfinished
    
**Architecture overview**
<p align="center">
   <img alt="Ubiquitous Architecture" src="img/ubiquitous-architecture.png" />
</p>

**Tech stack RoadMap**
----------------
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
------ 
- ***Angular*** [todo] Currently in development /Educating myself now on it/ 
- ***Redis*** [todo] Distributed caching
- ***Ocelot*** [todo] ApiGW
- ***Zipkin*** [todo] tracing
- ***Grafana*** [todo] metrics
- ***Prometheus*** [todo] metrics infrastructure
- ***ELK Stack*** [todo] logging and analysis 

**Services RoadMap**
-------
Services
- ***SmartStore Adapter*** Wholesale, source of data
- ***Fetch Service*** Fetches data from wholesales(many) and pushes newest items on bus
- ***Product Service*** Main domain aggregate service, handles products and its business logic
- ***Report Service*** Handles reports and its generation thanks to [Caracan](https://github.com/caracan-team)
- ***Notification Service*** Handles notifications and channels it by WebSocket
-------

**Modules**
- ***IntegrationEvent Log*** Shared integration events dbContext for each project
- ***EventBus RabbitMQ*** RawRabbit project encapsulating RabbitMQ asynchronous event bus
- ***Common*** Shared code, DI registration, snippets
-------

**Future Services**
- ***Subscription Service*** Handles logic of user's intention for events notification
- ***Identity Service*** Handles identification of user (rather wrapper over IdentityServer4)
- ***Auth Service*** Handles multi tenant authorization cases allowing to generate expression trees, translatable by LinqToSQL
-------

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Inspired by (Repos worth to see)
- [DevMentors](https://github.com/devmentors) and their great tutorial about .NET Core with top-class explanation
- [Dotnet](https://github.com/dotnet-architecture/eShopOnContainers) obviously, #1 architectural hats-off project
- [ASC-Lab](https://github.com/asc-lab/dotnetcore-microservices-poc) 

## License
[MIT](https://choosealicense.com/licenses/mit/)
