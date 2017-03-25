using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    [Collection("db")]
    public class ServiceCollectionExtensionsTests: IClassFixture<MongoHelper>
    {
        private readonly IServiceProvider provider;

        public ServiceCollectionExtensionsTests(MongoHelper helper)
        {
            provider = new ServiceCollection()
                .AddMongoUserStore(helper.ConfigureUserStoreOptions)
                .AddMongoRoleStore(helper.ConfigureRoleStoreOptions)
                .AddOptions()
                .BuildServiceProvider();
        }

        [Fact]
        public void CanResolveUserStore()
        {
            var result = provider.GetRequiredService<IUserStore<IdentityUser>>();
            Assert.IsAssignableFrom<MongoUserStore<IdentityUser>>(result);
        }

        [Fact]
        public void CanResolveRoleStore()
        {
            var result = provider.GetRequiredService<IRoleStore<IdentityRole>>();
            Assert.IsAssignableFrom<MongoRoleStore>(result);
        }
    }
}
