using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace PTrampert.AspNetCore.Identity.MongoDB.Configuration
{
    public class MongoUserStoreOptions<T> where T : IdentityUser
    {
        public MongoDatabaseOptions DatabaseOptions { get; set; }

        public string UsersCollectionName { get; set; } = "users";

        public bool ManageIndicies { get; set; }

        private IMongoCollection<T> users;
        internal IMongoCollection<T> Users
        {
            get
            {
                if (users == null)
                {
                    users = DatabaseOptions.Db.GetCollection<T>(UsersCollectionName);
                    if (ManageIndicies)
                    {
                        users.Indexes.CreateOne(
                            Builders<T>.IndexKeys.Ascending(u => u.Name),
                            new CreateIndexOptions<T>
                            {
                                Unique = true,
                                Sparse = false
                            });
                        users.Indexes.CreateOne(
                            Builders<T>.IndexKeys.Ascending(u => u.NormalizedName),
                            new CreateIndexOptions<T>
                            {
                                Unique = true,
                                Sparse = false
                            });
                        users.Indexes.CreateOne(
                            Builders<T>.IndexKeys.Ascending(u => u.NormalizedEmail),
                            new CreateIndexOptions<T>
                            {
                                Unique = true,
                                Sparse = false
                            });
                        users.Indexes.CreateOne(
                            Builders<T>.IndexKeys.Ascending(u => u.Roles), new CreateIndexOptions
                            {
                                Sparse = true,
                            });
                        users.Indexes.CreateOne(
                            Builders<T>.IndexKeys.Combine(
                                Builders<T>.IndexKeys.Ascending(
                                    new StringFieldDefinition<T>(
                                        $"{nameof(IdentityUser.Logins)}.{nameof(PersistedUserLoginInfo.LoginProvider)}")),
                                Builders<T>.IndexKeys.Ascending(
                                    new StringFieldDefinition<T>(
                                        $"{nameof(IdentityUser.Logins)}.{nameof(PersistedUserLoginInfo.ProviderKey)}"))
                            )
                        );
                        users.Indexes.CreateOne(
                            Builders<T>.IndexKeys.Combine(
                                Builders<T>.IndexKeys.Ascending(
                                    new StringFieldDefinition<T>(
                                        $"{nameof(IdentityUser.Claims)}.{nameof(PersistedClaim.Type)}")),
                                Builders<T>.IndexKeys.Ascending(
                                    new StringFieldDefinition<T>(
                                        $"{nameof(IdentityUser.Claims)}.{nameof(PersistedClaim.Value)}"))
                            )
                        );
                    }
                }
                return users;
            }
        }
    }
}
