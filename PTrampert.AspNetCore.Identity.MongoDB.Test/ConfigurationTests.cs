using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PTrampert.AspNetCore.Identity.MongoDB.Configuration;
using Xunit;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    public class ConfigurationTests
    {
        private readonly IServiceProvider provider;

        public ConfigurationTests()
        {
            var serviceCollection = new ServiceCollection();
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("testconfig.json")
                .Build();
            serviceCollection.Configure<MongoUserStoreOptions<IdentityUser>>(configuration.GetSection("IdentityOptions"));
            serviceCollection.Configure<MongoRoleStoreOptions<IdentityRole>>(configuration.GetSection("IdentityOptions"));
            provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ItLoadsMongoUserStoreOptionsCorrectly()
        {
            var result = new MongoUserStoreOptions<IdentityUser>();
            provider.GetService<IConfigureOptions<MongoUserStoreOptions<IdentityUser>>>().Configure(result);
            Assert.Equal("mongodb://localhost/", result.DatabaseOptions.ConnectionString);
            Assert.Equal("test", result.DatabaseOptions.Database);
            Assert.Equal("testUsers", result.UsersCollectionName);
            Assert.True(result.ManageIndicies);
        }

        [Fact]
        public void ItLoadsMongoRoleStoreOptionsCorrectly()
        {

            var result = new MongoRoleStoreOptions<IdentityRole>();
            provider.GetService<IConfigureOptions<MongoRoleStoreOptions<IdentityRole>>>().Configure(result);
            Assert.Equal("mongodb://localhost/", result.DatabaseOptions.ConnectionString);
            Assert.Equal("test", result.DatabaseOptions.Database);
            Assert.Equal("testRoles", result.RolesCollectionName);
            Assert.True(result.ManageIndicies);
        }
    }
}
