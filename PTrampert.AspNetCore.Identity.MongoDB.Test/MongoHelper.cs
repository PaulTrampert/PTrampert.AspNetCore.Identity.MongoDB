using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    public class MongoHelper : IDisposable
    {
        private static string DbName => $"IdTest-{BranchAndBuildHelper.BranchAndBuild}";

        public IMongoClient Client => new MongoClient("mongodb://localhost/");
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
