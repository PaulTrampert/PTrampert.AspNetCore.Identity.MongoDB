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
    public class UserStoreTests : IClassFixture<MongoHelper>, IDisposable
    {
        private IMongoDatabase db;
        private IMongoCollection<MongoIdentityUser> usersCollection;
        private MongoUserStore userStore;
        private MongoIdentityUser testUser;

        public UserStoreTests(MongoHelper mongoHelper)
        {
            testUser = new MongoIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "name",
                NormalizedName = "normalizedName"
            };
            db = mongoHelper.Database;
            usersCollection = mongoHelper.Users;
            userStore = new MongoUserStore(usersCollection);
        }

        [Fact]
        public async Task CanCreateUser()
        {
            var result = await userStore.CreateAsync(testUser, default(CancellationToken));
            Assert.Equal(IdentityResult.Success, result);
            var storedUser = (await usersCollection.FindAsync(u => u.Id == testUser.Id)).Single();
            Assert.NotNull(storedUser);
            Assert.True(testUser.PropertiesEqual(storedUser));
        }

        [Fact]
        public async Task CanDeleteUser()
        {
            await userStore.CreateAsync(testUser, default(CancellationToken));
            var storedUser = (await usersCollection.FindAsync(u => u.Id == testUser.Id)).Single();
            Assert.NotNull(storedUser);
            var result = await userStore.DeleteAsync(testUser, default(CancellationToken));
            Assert.Equal(IdentityResult.Success, result);
            storedUser = (await usersCollection.FindAsync(u => u.Id == testUser.Id)).SingleOrDefault();
            Assert.Null(storedUser);
        }

        [Fact]
        public async Task CanFindById()
        {
            await userStore.CreateAsync(testUser, default(CancellationToken));
            var result = await userStore.FindByIdAsync(testUser.Id, default(CancellationToken));
            Assert.True(testUser.PropertiesEqual(result));
        }

        [Fact]
        public async Task FindByIdReturnsNullWhenNotFound()
        {
            var result = await userStore.FindByIdAsync(testUser.Id, CancellationToken.None);
            Assert.Null(result);
        }

        [Fact]
        public async Task CanFindByName()
        {
            await userStore.CreateAsync(testUser, default(CancellationToken));
            var result = await userStore.FindByNameAsync(testUser.NormalizedName, default(CancellationToken));
            Assert.True(testUser.PropertiesEqual(result));
        }

        [Fact]
        public async Task FindByNameReturnsNullWhenNotFound()
        {
            var result = await userStore.FindByNameAsync(testUser.NormalizedName, default(CancellationToken));
            Assert.Null(result);
        }

        [Fact]
        public async Task GetNormalizedUserNameReturnsNormalizedName()
        {
            var result = await userStore.GetNormalizedUserNameAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.NormalizedName, result);
        }

        [Fact]
        public async Task SetNormalizedUserNameSetsTheNormalizedUserName()
        {
            await userStore.SetNormalizedUserNameAsync(testUser, "somethingElse", default(CancellationToken));
            Assert.Equal("somethingElse", testUser.NormalizedName);
        }

        [Fact]
        public async Task GetUserIdReturnsId()
        {
            var result = await userStore.GetUserIdAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.Id, result);
        }

        [Fact]
        public async Task GetUserNameReturnsName()
        {
            var result = await userStore.GetUserNameAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.Name, result);
        }

        [Fact]
        public async Task SetUserNameSetsTheNormalizedUserName()
        {
            await userStore.SetUserNameAsync(testUser, "somethingElse", default(CancellationToken));
            Assert.Equal("somethingElse", testUser.Name);
        }

        [Fact]
        public async Task CanUpdateAnExistingUser()
        {
            await userStore.CreateAsync(testUser, default(CancellationToken));
            testUser.Name = "something good";
            testUser.NormalizedName = "something bad";
            var result = await userStore.UpdateAsync(testUser, default(CancellationToken));
            Assert.Equal(IdentityResult.Success, result);
            var storedUser = await userStore.FindByIdAsync(testUser.Id, default(CancellationToken));
            Assert.True(testUser.PropertiesEqual(storedUser));
        }

        public void Dispose()
        {
            db.DropCollection(usersCollection.CollectionNamespace.CollectionName);
        }
    }
}
