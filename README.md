# PTrampert.AspNetCore.Identity.MongoDB
Master Build: ![Build Status](https://jenkins.ptrampert.com/buildStatus/icon?job=Paul%20Trampert/PTrampert.AspNetCore.Identity.MongoDB/master)

MongoDB data store for AspNetCore.Identity.

## Usage

Config file format:

```json
{
    "DatabaseOptions": {
        "ConnectionString": "mongodb://localhost/",
        "Database": "testdb"
    },
    "UsersCollection": "users",
    "RolesCollection": "roles",
    "ManageIndicies": true
}
```

*NOTE:* The DatabaseOptions config section can be used with [IdentityServer4.MongoDB](https://github.com/diogodamiani/IdentityServer4.MongoDB/blob/dev/src/IdentityServer4.MongoDB/Configuration/MongoDBConfiguration.cs).

IServiceCollection extensions:
```csharp
AddMongoUserStore<T>(IConfiguration config)
AddMongoUserStore<T>(Action<MongoUserStoreOptions<T>> config)
AddMongoRoleStore(IConfiguration config)
AddMongoRoleStore(Action<MongoRoleStoreOptions config)
```
