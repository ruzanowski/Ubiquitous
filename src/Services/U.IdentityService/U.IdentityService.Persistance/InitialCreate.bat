dotnet ef migrations add Nickname --context IdentityContext --output-dir Migrations/Identity
dotnet ef database update --context IdentityContext
