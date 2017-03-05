using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoClient(this IServiceCollection services, string mongoConnectionString = "mongodb://localhost/")
        {
            var mongoUrl = new MongoUrl(mongoConnectionString);
            services.AddMongoClient(mongoUrl);
        }

        public static void AddMongoClient(this IServiceCollection services, MongoUrl mongoUrl)
        {
            var clientSettings = MongoClientSettings.FromUrl(mongoUrl);
            services.AddMongoClient(clientSettings);
        }

        public static void AddMongoClient(this IServiceCollection services, MongoClientSettings settings)
        {
            var client = new MongoClient(settings);
            services.Add(new ServiceDescriptor(typeof(IMongoClient), client));
        }

        public static void AddMongoDatabase(this IServiceCollection services, string databaseName = "Identity")
        {
            services.AddMongoDatabase(databaseName, new MongoDatabaseSettings());
        }

        public static void AddMongoDatabase(this IServiceCollection services, string databaseName, MongoDatabaseSettings settings)
        {
            var descriptor = new ServiceDescriptor(typeof(IMongoDatabase), p => p.GetService<IMongoClient>().GetDatabase(databaseName, settings), ServiceLifetime.Singleton);
            services.Add(descriptor);
        }

        public static void AddMongoUserStore(this IServiceCollection services, string usersCollectionName = "users", bool manageIndicies = true)
        {
            var serviceDescriptor = new ServiceDescriptor(
                typeof(IUserStore<IdentityUser>), 
                p =>
                {
                    var mongoCollection =
                        p.GetService<IMongoDatabase>().GetCollection<IdentityUser>(usersCollectionName);
                    if (manageIndicies)
                    {
                        mongoCollection.Indexes.CreateOne(
                            Builders<IdentityUser>.IndexKeys.Ascending(u => u.Name),
                            new CreateIndexOptions<IdentityUser>
                            {
                                Unique = true,
                                Sparse = false
                            });
                        mongoCollection.Indexes.CreateOne(
                            Builders<IdentityUser>.IndexKeys.Ascending(u => u.NormalizedName),
                            new CreateIndexOptions<IdentityUser>
                            {
                                Unique = true,
                                Sparse = false
                            });
                        mongoCollection.Indexes.CreateOne(
                            Builders<IdentityUser>.IndexKeys.Ascending(u => u.NormalizedEmail),
                            new CreateIndexOptions<IdentityUser>
                            {
                                Unique = true,
                                Sparse = false
                            });
                        mongoCollection.Indexes.CreateOne(
                            Builders<IdentityUser>.IndexKeys.Ascending(u => u.Roles), new CreateIndexOptions
                            {
                                Sparse = true,
                            });
                        mongoCollection.Indexes.CreateOne(
                            Builders<IdentityUser>.IndexKeys.Combine(
                                Builders<IdentityUser>.IndexKeys.Ascending(
                                    new StringFieldDefinition<IdentityUser>(
                                        $"{nameof(IdentityUser.Logins)}.{nameof(PersistedUserLoginInfo.LoginProvider)}")),
                                Builders<IdentityUser>.IndexKeys.Ascending(
                                    new StringFieldDefinition<IdentityUser>(
                                        $"{nameof(IdentityUser.Logins)}.{nameof(PersistedUserLoginInfo.ProviderKey)}"))
                            )
                        );
                        mongoCollection.Indexes.CreateOne(
                            Builders<IdentityUser>.IndexKeys.Combine(
                                Builders<IdentityUser>.IndexKeys.Ascending(
                                    new StringFieldDefinition<IdentityUser>(
                                        $"{nameof(IdentityUser.Claims)}.{nameof(PersistedClaim.Type)}")),
                                Builders<IdentityUser>.IndexKeys.Ascending(
                                    new StringFieldDefinition<IdentityUser>(
                                        $"{nameof(IdentityUser.Claims)}.{nameof(PersistedClaim.Value)}"))
                            )
                        );
                    }
                    return new MongoUserStore(mongoCollection);
                }, 
                ServiceLifetime.Scoped);
            services.Add(serviceDescriptor);
        }

        public static void AddMongoRolesStore(this IServiceCollection services, string rolesCollectionName = "roles", bool manageIndicies = true)
        {
            var serviceDescriptor = new ServiceDescriptor(
                typeof(IRoleStore<IdentityRole>),
                p =>
                {
                    var rolesCollection = p.GetService<IMongoDatabase>().GetCollection<IdentityRole>(rolesCollectionName);
                    if (manageIndicies)
                    {
                        rolesCollection.Indexes.CreateOne(
                            Builders<IdentityRole>.IndexKeys.Ascending(r => r.NormalizedName), new CreateIndexOptions
                            {
                                Unique = true,
                                Sparse = false
                            });
                    }
                    return new MongoRoleStore(rolesCollection);
                }, 
                ServiceLifetime.Scoped);
            services.Add(serviceDescriptor);
        }
    }
}
