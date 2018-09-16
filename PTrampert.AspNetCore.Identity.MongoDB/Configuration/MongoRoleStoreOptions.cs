using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace PTrampert.AspNetCore.Identity.MongoDB.Configuration
{
    public class MongoRoleStoreOptions
    {
        public MongoDatabaseOptions DatabaseOptions { get; set; } = new MongoDatabaseOptions();

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
                        var keys = Builders<IdentityRole>.IndexKeys.Ascending(r => r.NormalizedName);
                        var opts = new CreateIndexOptions
                        {
                            Unique = true,
                            Sparse = false,
                            Background = true
                        };
                        roles.Indexes.CreateOne(new CreateIndexModel<IdentityRole>(keys, opts));
                    }
                }
                return roles;
            }
        }
    }
}
