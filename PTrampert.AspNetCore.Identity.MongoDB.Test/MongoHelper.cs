using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using PTrampert.AspNetCore.Identity.MongoDB.Configuration;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    public class MongoHelper : IDisposable
    {
        private static string DbName => $"IdTest-{BranchAndBuildHelper.BranchAndBuild}";

        private static string ConnectionString => Environment.GetEnvironmentVariable("MONGO_CONNECTION") ?? "mongodb://localhost/";

        public Action<MongoUserStoreOptions<IdentityUser>> ConfigureUserStoreOptions = opts =>
        {
            opts.DatabaseOptions.Database = DbName;
            opts.DatabaseOptions.ConnectionString = ConnectionString;
            opts.ManageIndicies = true;
        };

        public Action<MongoRoleStoreOptions> ConfigureRoleStoreOptions = opts =>
        {
            opts.DatabaseOptions.Database = DbName;
            opts.DatabaseOptions.ConnectionString = ConnectionString;
            opts.ManageIndicies = true;
        };

        public IMongoClient Client => new MongoClient(ConnectionString);
        public IMongoDatabase Database => Client.GetDatabase(DbName);
        public IMongoCollection<IdentityUser> Users => Database.GetCollection<IdentityUser>("users");
        public IMongoCollection<IdentityRole> Roles => Database.GetCollection<IdentityRole>("roles");

        public void Dispose()
        {
            try
            {
                Client.DropDatabase(DbName);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
