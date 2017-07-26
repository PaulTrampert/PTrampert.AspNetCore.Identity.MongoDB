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
        /// <summary>
        /// Adds <see cref="MongoUserStore{T}"/> as the service for <see cref="IUserStore{TUser}"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// services.AddMongoUserStore&lt;IdentityUser&gt;(opts => 
        /// {
        ///     opts.DatabaseOptions.ConnectionString = "mongodb://localhost/";
        ///     opts.DatabaseOptions.Database = "testdb";
        ///     opts.UsersCollectionName = "users";
        ///     opts.ManageIndicies = true;
        /// });
        /// </code>
        /// </example>
        /// <typeparam name="T">The user type. Must inherit from <see cref="IdentityUser"/></typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Action setting the <see cref="MongoUserStoreOptions{T}"/>.</param>
        /// <returns>The service collection so further calls can be chained.</returns>
        public static IServiceCollection AddMongoUserStore<T>(this IServiceCollection services, Action<MongoUserStoreOptions<T>> configure) where T : IdentityUser
        {
            services.Configure(configure);
            services.AddScoped<IUserStore<T>, MongoUserStore<T>>();
            return services;
        }

        /// <summary>
        /// Adds <see cref="MongoUserStore{T}"/> as the service for <see cref="IUserStore{TUser}"/>.
        /// </summary>
        /// <example>
        /// Using the following configuration file:
        /// <code>
        /// {
        ///    "IdentityOptions": {
        ///      "DatabaseOptions": {
        ///        "ConnectionString": "mongodb://localhost/",
        ///        "Database": "testdb"
        ///     },
        ///     "UsersCollectionName": "users",
        ///     "ManageIndicies": true
        ///   }
        /// }
        /// </code>
        /// <code>
        /// var config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
        /// services.AddMongoUserStore&lt;IdentityUser&gt;(config.GetSection("IdentityOptions"));
        /// </code>
        /// </example>
        /// <typeparam name="T">The user type. Must inherit from <see cref="IdentityUser"/></typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration section to setup <see cref="MongoUserStoreOptions{T}"/> from.</param>
        /// <returns>The service collection so further calls can be chained.</returns>
        public static IServiceCollection AddMongoUserStore<T>(this IServiceCollection services, IConfiguration configuration) where T: IdentityUser
        {
            services.Configure<MongoUserStoreOptions<T>>(configuration);
            services.AddScoped<IUserStore<T>, MongoUserStore<T>>();
            return services;
        }

        /// <summary>
        /// Adds <see cref="MongoRoleStore"/> as the service for <see cref="IRoleStore{IdentityRole}"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// services.AddMongoRoleStore(opts => 
        /// {
        ///     opts.DatabaseOptions.ConnectionString = "mongodb://localhost/";
        ///     opts.DatabaseOptions.Database = "testdb";
        ///     opts.RolesCollectionName = "roles";
        ///     opts.ManageIndicies = true;
        /// });
        /// </code>
        /// </example>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Action setting the <see cref="MongoUserStoreOptions{T}"/>.</param>
        /// <returns>The service collection so further calls can be chained.</returns>
        public static IServiceCollection AddMongoRoleStore(this IServiceCollection services,
            Action<MongoRoleStoreOptions> configure)
        {
            services.Configure(configure);
            services.AddScoped<IRoleStore<IdentityRole>, MongoRoleStore>();
            return services;
        }

        /// <summary>
        /// Adds <see cref="MongoRoleStoreOptions"/> as the service for <see cref="IRoleStore{IdentityRole}"/>.
        /// </summary>
        /// <example>
        /// Using the following configuration file:
        /// <code>
        /// {
        ///    "IdentityOptions": {
        ///      "DatabaseOptions": {
        ///        "ConnectionString": "mongodb://localhost/",
        ///        "Database": "testdb"
        ///     },
        ///     "RolesCollectionName": "roles",
        ///     "ManageIndicies": true
        ///   }
        /// }
        /// </code>
        /// <code>
        /// var config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
        /// services.AddMongoRoleStore(config.GetSection("IdentityOptions"));
        /// </code>
        /// </example>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration section to setup <see cref="MongoRoleStoreOptions"/> from.</param>
        /// <returns>The service collection so further calls can be chained.</returns>
        public static IServiceCollection AddMongoRoleStore(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoRoleStoreOptions>(configuration);
            services.AddScoped<IRoleStore<IdentityRole>, MongoRoleStore>();
            return services;
        }
    }
}
