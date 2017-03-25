using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace PTrampert.AspNetCore.Identity.MongoDB.Configuration
{
    public class MongoDatabaseOptions
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        private IMongoClient client;

        private IMongoDatabase database;

        internal IMongoClient Client => client ?? (client = new MongoClient(ConnectionString));

        internal IMongoDatabase Db => database ?? (database = Client.GetDatabase(Database));
    }
}
