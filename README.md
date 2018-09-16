# PTrampert.AspNetCore.Identity.MongoDB
Dev Build: [![Build Status](https://jenkins.ptrampert.com/buildStatus/icon?job=PaulTrampert/PTrampert.AspNetCore.Identity.MongoDB/dev)](https://jenkins.ptrampert.com/job/PaulTrampert/PTrampert.AspNetCore.Identity.MongoDB/dev)

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
