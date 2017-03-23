# PTrampert.AspNetCore.Identity.MongoDB
Master Build: ![Build Status](https://jenkins.ptrampert.com/buildStatus/icon?job=GitHub%20PaulTrampert/PTrampert.AspNetCore.Identity.MongoDB/master)

MongoDB data store for AspNetCore.Identity.

## Usage

To register the mongo identity store providers, the following extension methods to IServiceCollection have been provided:
* `AddMongoClient()`
* `AddMongoDatabase()`
* `AddMongoUserStore()`
* `AddMongoRolesStore()`

Each of these methods has parameterless forms that provide reasonable default values, though you will probably want to override the mongo connection string provided as the default for AddMongoClient().
