using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PTrampert.AspNetCore.Identity.MongoDB.Configuration;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoUserStore<T>(this IServiceCollection services, Action<MongoUserStoreOptions<T>> configure) where T : IdentityUser
        {
            services.Configure(configure);
            services.AddSingleton<IUserStore<T>, MongoUserStore<T>>();
            return services;
        }

        public static IServiceCollection AddMongoUserStore<T>(this IServiceCollection services, IConfiguration configuration) where T: IdentityUser
        {
            services.Configure<MongoUserStoreOptions<T>>(configuration);
            services.AddSingleton<IUserStore<T>, MongoUserStore<T>>();
            return services;
        }

        public static IServiceCollection AddMongoRoleStore(this IServiceCollection services,
            Action<MongoRoleStoreOptions> configure)
        {
            services.Configure(configure);
            services.AddSingleton<IRoleStore<IdentityRole>, MongoRoleStore>();
            return services;
        }

        public static IServiceCollection AddMongoRoleStore(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoRoleStoreOptions>(configuration);
            services.AddSingleton<IRoleStore<IdentityRole>, MongoRoleStore>();
            return services;
        }
    }
}
