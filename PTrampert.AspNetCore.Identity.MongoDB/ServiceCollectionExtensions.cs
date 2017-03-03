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

        public static void AddMongoUserStore(this IServiceCollection services, string usersCollectionName = "users")
        {
            var serviceDescriptor = new ServiceDescriptor(
                typeof(IUserStore<IdentityUser>), 
                p => new MongoUserStore(p.GetService<IMongoDatabase>().GetCollection<IdentityUser>(usersCollectionName)), 
                ServiceLifetime.Scoped);
            services.Add(serviceDescriptor);
        }

        public static void AddMongoRolesStore(this IServiceCollection services, string rolesCollectionName = "roles")
        {
            var serviceDescriptor = new ServiceDescriptor(
                typeof(IRoleStore<IdentityRole>),
                p => new MongoRoleStore(p.GetService<IMongoDatabase>().GetCollection<IdentityRole>(rolesCollectionName)), 
                ServiceLifetime.Scoped);
            services.Add(serviceDescriptor);
        }
    }
}
