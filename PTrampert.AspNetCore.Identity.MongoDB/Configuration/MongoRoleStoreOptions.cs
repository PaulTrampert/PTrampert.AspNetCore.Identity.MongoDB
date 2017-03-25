using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace PTrampert.AspNetCore.Identity.MongoDB.Configuration
{
    public class MongoRoleStoreOptions
    {
        public MongoDatabaseOptions DatabaseOptions { get; set; }

        public string RolesCollectionName { get; set; } = "roles";

        public bool ManageIndicies { get; set; }

        private IMongoCollection<IdentityRole> roles;
        internal IMongoCollection<IdentityRole> Roles
        {
            get
            {
                if (roles == null)
                {
                    roles = DatabaseOptions.Db.GetCollection<IdentityRole>(RolesCollectionName);
                    if (ManageIndicies)
                    {
                        roles.Indexes.CreateOne(
                            Builders<IdentityRole>.IndexKeys.Ascending(r => r.NormalizedName), new CreateIndexOptions
                            {
                                Unique = true,
                                Sparse = false,
                                Background = true
                            });
                    }
                }
                return roles;
            }
        }
    }
}
