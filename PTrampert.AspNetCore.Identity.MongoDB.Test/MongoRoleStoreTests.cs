using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Xunit;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    [Collection("db")]
    public class MongoRoleStoreTests : IClassFixture<MongoHelper>, IDisposable
    {
        private IMongoDatabase db;
        private IMongoCollection<IdentityRole> roles;
        private MongoRoleStore subject;
        private IdentityRole testRole;

        public MongoRoleStoreTests(MongoHelper helper)
        {
            db = helper.Database;
            roles = helper.Roles;
            subject = new MongoRoleStore(roles);
            testRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "testRole",
                NormalizedName = "testNorm"
            };
        }

        public void Dispose()
        {
            db.DropCollection(roles.CollectionNamespace.CollectionName);
        }

        [Fact]
        public async Task CanCreateRole()
        {
            var result = await subject.CreateAsync(testRole, default(CancellationToken));
            Assert.Equal(IdentityResult.Success, result);
            var insertedRole = (await roles.FindAsync(r => r.Id == testRole.Id)).SingleOrDefault();
            Assert.NotNull(insertedRole);
            Assert.True(testRole.PropertiesEqual(insertedRole));
        }

        [Fact]
        public async Task CanUpdateRole()
        {
            var result = await subject.CreateAsync(testRole, default(CancellationToken));
            Assert.Equal(IdentityResult.Success, result);
            testRole.Name = "otherName";
            var insertedRole = (await roles.FindAsync(r => r.Id == testRole.Id)).SingleOrDefault();
            Assert.NotNull(insertedRole);
            Assert.False(testRole.PropertiesEqual(insertedRole));
            result = await subject.UpdateAsync(testRole, default(CancellationToken));
            Assert.Equal(IdentityResult.Success, result);
            insertedRole = (await roles.FindAsync(r => r.Id == testRole.Id)).SingleOrDefault();
            Assert.NotNull(insertedRole);
            Assert.True(testRole.PropertiesEqual(insertedRole));
        }

        [Fact]
        public async Task CanRemoveRole()
        {
            await subject.CreateAsync(testRole, default(CancellationToken));
            var insertedRole = (await roles.FindAsync(r => r.Id == testRole.Id)).SingleOrDefault();
            Assert.NotNull(insertedRole);
            var result = await subject.DeleteAsync(testRole, default(CancellationToken));
            Assert.Equal(IdentityResult.Success, result);
            insertedRole = (await roles.FindAsync(r => r.Id == testRole.Id)).SingleOrDefault();
            Assert.Null(insertedRole);
        }
    }
}
