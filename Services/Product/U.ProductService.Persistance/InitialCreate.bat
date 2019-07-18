dotnet ef migrations add InitialCreate --context ProductContext --output-dir Migrations/Product
dotnet ef migrations add InitialCreate --context IntegrationEventLogContext --output-dir Migrations/Event
dotnet ef database update --context ProductContext
dotnet ef database update --context IntegrationEventLogContext