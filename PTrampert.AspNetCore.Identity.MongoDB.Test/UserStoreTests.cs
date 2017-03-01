using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Xunit;
using System.Security.Claims;

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
                LockoutEndDate = new DateTimeOffset(DateTime.Now),
                PhoneNumber = "222222222222"
            };
            testUser.AddLogin(new PersistedUserLoginInfo("gwar", "123"));
            testUser.AddClaim(new PersistedClaim("test", "data"));
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
            await userStore.AddLoginAsync(testUser, new UserLoginInfo("google", "gawg", "Google"), default(CancellationToken));
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

        [Fact]
        public async Task CanAddClaims()
        {
            var claims = new[]
            {
                new Claim("Fake", "Claim"),
                new Claim("Bogus", "Data")
            };
            await userStore.AddClaimsAsync(testUser, claims, default(CancellationToken));
            Assert.Contains(testUser.Claims, c => c.Type == "Fake" && c.Value == "Claim");
            Assert.Contains(testUser.Claims, c => c.Type == "Bogus" && c.Value == "Data");
        }

        [Fact]
        public async Task CanRemoveClaims()
        {
            await userStore.RemoveClaimsAsync(testUser, testUser.Claims.Select(c => new Claim(c.Type, c.Value)), default(CancellationToken));
            Assert.Empty(testUser.Claims);
        }

        [Fact]
        public async Task CanReplaceClaim()
        {
            await userStore.ReplaceClaimAsync(testUser, new Claim("test", "data"), new Claim("bogus", "data"), default(CancellationToken));
            Assert.Contains(new PersistedClaim("bogus", "data"), testUser.Claims);
            Assert.DoesNotContain(new PersistedClaim("test", "data"), testUser.Claims);
        }

        [Fact]
        public async Task CanGetClaims()
        {
            var result = await userStore.GetClaimsAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.Claims, result.Select(c => new PersistedClaim(c)));
        }

        [Fact]
        public async Task CanGetUsersForClaim()
        {
            await userStore.CreateAsync(testUser, default(CancellationToken));
            var result = await userStore.GetUsersForClaimAsync(new Claim("test", "data"), default(CancellationToken));
            Assert.True(result.First().PropertiesEqual(testUser));
        }

        [Fact]
        public async Task CanGetLockoutEndDate()
        {
            var result = await userStore.GetLockoutEndDateAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.LockoutEndDate, result);
        }

        [Fact]
        public async Task CanSetLockoutEndDate()
        {
            await userStore.SetLockoutEndDateAsync(testUser, null, default(CancellationToken));
            Assert.Null(testUser.LockoutEndDate);
        }

        [Theory]
        [InlineData(2, 3)]
        [InlineData(20, 21)]
        [InlineData(42, 43)]
        public async Task CanIncrementAccessFailedCount(int start, int expectedResult)
        {
            testUser.AccessFailedCount = start;
            await userStore.IncrementAccessFailedCountAsync(testUser, default(CancellationToken));
            Assert.Equal(expectedResult, testUser.AccessFailedCount);
        }


        [Theory]
        [InlineData(2)]
        [InlineData(20)]
        [InlineData(42)]
        public async Task CanResetAccessFailedCount(int accessFailedCount)
        {
            testUser.AccessFailedCount = accessFailedCount;
            await userStore.ResetAccessFailedCountAsync(testUser, default(CancellationToken));
            Assert.Equal(0, testUser.AccessFailedCount);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(20)]
        [InlineData(42)]
        public async Task CanGetAccessFailedCount(int accessFailedCount)
        {
            testUser.AccessFailedCount = accessFailedCount;
            var result = await userStore.GetAccessFailedCountAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.AccessFailedCount, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CanGetLockoutEnabled(bool lockoutEnabled)
        {
            testUser.LockoutEnabled = lockoutEnabled;
            var result = await userStore.GetLockoutEnabledAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.LockoutEnabled, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CanSetLockoutEnabled(bool lockoutEnabled)
        {
            await userStore.SetLockoutEnabledAsync(testUser, lockoutEnabled, default(CancellationToken));
            Assert.Equal(lockoutEnabled, testUser.LockoutEnabled);
        }

        [Theory]
        [InlineData("123-456-6677")]
        [InlineData("5555555555")]
        public async Task CanSetUserPhoneNumber(string phoneNumber)
        {
            await userStore.SetPhoneNumberAsync(testUser, phoneNumber, default(CancellationToken));
            Assert.Equal(phoneNumber, testUser.PhoneNumber);
        }

        [Theory]
        [InlineData("123-456-6677")]
        [InlineData("5555555555")]
        public async Task CanGetUserPhoneNumber(string phoneNumber)
        {
            testUser.PhoneNumber = phoneNumber;
            var result = await userStore.GetPhoneNumberAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.PhoneNumber, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CanSetPhoneNumberConfirmed(bool confirmed)
        {
            await userStore.SetPhoneNumberConfirmedAsync(testUser, confirmed, default(CancellationToken));
            Assert.Equal(confirmed, testUser.PhoneNumberConfirmed);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CanGetPhoneNumberConfirmed(bool confirmed)
        {
            testUser.PhoneNumberConfirmed = confirmed;
            var result = await userStore.GetPhoneNumberConfirmedAsync(testUser, default(CancellationToken));
            Assert.Equal(testUser.PhoneNumberConfirmed, result);
        }

        public void Dispose()
        {
            db.DropCollection(usersCollection.CollectionNamespace.CollectionName);
        }
    }
}
