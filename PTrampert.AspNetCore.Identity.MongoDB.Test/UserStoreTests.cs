using System;
using System.Collections.Generic;
using System.Linq;
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
        private IMongoCollection<IdentityUser> usersCollection;
        private MongoUserStore userStore;
        private IdentityUser testUser;

        public UserStoreTests(MongoHelper mongoHelper)
        {
            testUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "name",
                NormalizedName = "normalizedName",
                PasswordHash = "hashypash",
                Email = "test@tester.com",
                NormalizedEmail = "normal@norm.com",
                EmailConfirmed = true,
                SecurityStamp = "stampy",
                Logins = new List<UserLoginInfo> { new UserLoginInfo { ProviderKey = "123", LoginProvider = "gwar"} }
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

        [Fact]
        public async Task CanGetPasswordHash()
        {
            var result = await userStore.GetPasswordHashAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.PasswordHash, result);
        }

        [Fact]
        public async Task CanSetPasswordHash()
        {
            await userStore.SetPasswordHashAsync(testUser, "somehash", default(CancellationToken));
            Assert.Equal("somehash", testUser.PasswordHash);
        }

        [Fact]
        public async Task HasPasswordReturnsTrueWhenPasswordHashIsSet()
        {
            Assert.True(await userStore.HasPasswordAsync(testUser, default(CancellationToken)));
        }

        [Fact]
        public async Task HasPasswordReturnsFalseWhenPasswordHashIsNull()
        {
            testUser.PasswordHash = null;
            Assert.False(await userStore.HasPasswordAsync(testUser, default(CancellationToken)));
        }

        [Fact]
        public async Task HasPasswordReturnsFalseWhenPasswordHashIsEmpty()
        {
            testUser.PasswordHash = "";
            Assert.False(await userStore.HasPasswordAsync(testUser, default(CancellationToken)));
        }

        [Fact]
        public async Task CanGetEmail()
        {
            Assert.Equal(testUser.Email, await userStore.GetEmailAsync(testUser, default(CancellationToken)));
        }

        [Fact]
        public async Task CanSetUserEmail()
        {
            await userStore.SetEmailAsync(testUser, "some@email.com", default(CancellationToken));
            Assert.Equal("some@email.com", testUser.Email);
        }

        [Fact]
        public async Task CanGetNormalizedEmail()
        {
            Assert.Equal(testUser.NormalizedEmail, await userStore.GetNormalizedEmailAsync(testUser, default(CancellationToken)));
        }

        [Fact]
        public async Task CanSetNormalizedEmail()
        {
            await userStore.SetNormalizedEmailAsync(testUser, "some@NormalizedEmail.com", default(CancellationToken));
            Assert.Equal("some@NormalizedEmail.com", testUser.NormalizedEmail);
        }

        [Fact]
        public async Task CanGetEmailConfirmed()
        {
            Assert.True(await userStore.GetEmailConfirmedAsync(testUser, default(CancellationToken)));
        }

        [Fact]
        public async Task CanSetEmailConfirmed()
        {
            await userStore.SetEmailConfirmedAsync(testUser, false, default(CancellationToken));
            Assert.False(testUser.EmailConfirmed);
        }

        [Fact]
        public async Task CanFindByEmail()
        {
            await userStore.CreateAsync(testUser, default(CancellationToken));
            var result = await userStore.FindByEmailAsync(testUser.NormalizedEmail, default(CancellationToken));
            Assert.True(testUser.PropertiesEqual(result));
        }

        [Fact]
        public async Task FindByEmailReturnsNullWhenUserDoesntExist()
        {
            var result = await userStore.FindByEmailAsync(testUser.NormalizedEmail, default(CancellationToken));
            Assert.Null(result);
        }

        [Fact]
        public async Task CanGetSecurityStamp()
        {
            var result = await userStore.GetSecurityStampAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.SecurityStamp, result);
        }

        [Fact]
        public async Task CanSetSecurityStamp()
        {
            await userStore.SetSecurityStampAsync(testUser, "somestamp2", default(CancellationToken));
            Assert.Equal("somestamp2", testUser.SecurityStamp);
        }

        [Fact]
        public async Task CanAddLoginInfo()
        {
            await userStore.AddLoginAsync(testUser, new Microsoft.AspNetCore.Identity.UserLoginInfo("google", "gawg", "Google"), default(CancellationToken));
            Assert.Contains(testUser.Logins, l => l.LoginProvider == "google" && l.ProviderKey == "gawg" && l.ProviderDisplayName == "Google");
        }

        [Fact]
        public async Task CanRemoveLoginInfo()
        {
            await userStore.RemoveLoginAsync(testUser, "gwar", "123", default(CancellationToken));
            Assert.DoesNotContain(testUser.Logins, l => l.LoginProvider == "gwar");
        }

        [Fact]
        public async Task CanGetLogins()
        {
            var result = await userStore.GetLoginsAsync(testUser, default(CancellationToken));
            Assert.True(result[0].ProviderKey == "123" && result[0].LoginProvider == "gwar");
        }

        [Fact]
        public async Task CanFindByLogin()
        {
            await userStore.CreateAsync(testUser, default(CancellationToken));
            var result = await userStore.FindByLoginAsync("gwar", "123", default(CancellationToken));
            Assert.True(testUser.PropertiesEqual(result));
        }

        public void Dispose()
        {
            db.DropCollection(usersCollection.CollectionNamespace.CollectionName);
        }
    }
}
