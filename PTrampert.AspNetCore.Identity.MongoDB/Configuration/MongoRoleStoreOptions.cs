using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace PTrampert.AspNetCore.Identity.MongoDB.Configuration
{
    public class MongoRoleStoreOptions<TRole>
        where TRole : IdentityRole
    {
        public MongoDatabaseOptions DatabaseOptions { get; set; } = new MongoDatabaseOptions();

        public string RolesCollectionName { get; set; } = "roles";

        public bool ManageIndicies { get; set; }

        private IMongoCollection<TRole> roles;
        internal IMongoCollection<TRole> Roles
        {
            get
            {
                if (roles == null)
                {
                    roles = DatabaseOptions.Db.GetCollection<TRole>(RolesCollectionName);
                    if (ManageIndicies)
                    {
                        var keys = Builders<TRole>.IndexKeys.Ascending(r => r.NormalizedName);
                        var opts = new CreateIndexOptions
                        {
                            Unique = true,
                            Sparse = false,
                            Background = true
                        };
                        roles.Indexes.CreateOne(new CreateIndexModel<TRole>(keys, opts));
                    }
                }
                return roles;
            }
        }
    }
}
