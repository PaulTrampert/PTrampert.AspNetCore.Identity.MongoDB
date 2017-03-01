using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class MongoUserStore :
        IUserLoginStore<IdentityUser>,
        IUserClaimStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>,
        IUserSecurityStampStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserLockoutStore<IdentityUser>,
        IUserPhoneNumberStore<IdentityUser>,
        IQueryableUserStore<IdentityUser>,
        IUserAuthenticationTokenStore<IdentityUser>,
        IUserTwoFactorStore<IdentityUser>
    {
        private IMongoCollection<IdentityUser> users;

        public IQueryable<IdentityUser> Users => users.AsQueryable();

        public MongoUserStore(IMongoCollection<IdentityUser> users)
        {
            this.users = users;
        }


        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            await users.InsertOneAsync(user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            await users.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            users = null;
        }

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return users.Find(u => u.Id == userId).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return users.Find(u => u.NormalizedName == normalizedUserName).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedName);
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.NormalizedName = normalizedName, cancellationToken);
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Name = userName, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            await users.FindOneAndReplaceAsync(u => u.Id == user.Id, user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.PasswordHash = passwordHash, cancellationToken);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetEmailAsync(IdentityUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Email = email, cancellationToken);
        }

        public Task<string> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.EmailConfirmed = confirmed, cancellationToken);
        }

        public async Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return (await users.FindAsync(u => u.NormalizedEmail == normalizedEmail, null, cancellationToken)).SingleOrDefault();
        }

        public Task<string> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(IdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.NormalizedEmail = normalizedEmail, cancellationToken);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.SecurityStamp = stamp, cancellationToken);
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AddLogin(new PersistedUserLoginInfo(login)), cancellationToken);
        }

        public Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveLogin(loginProvider, providerKey), cancellationToken);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Logins.Select(li => li.ToUserLoginInfo()).ToList() as IList<UserLoginInfo>);
        }

        public async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return (await users.FindAsync(u => u.Logins.Any(li => li.LoginProvider == loginProvider && li.ProviderKey == providerKey), null, cancellationToken)).SingleOrDefault();
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Claims.Select(c => new Claim(c.Type, c.Value)).ToList() as IList<Claim>);
        }

        public Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AddClaims(claims.Select(c => new PersistedClaim(c))), cancellationToken);
        }

        public Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.ReplaceClaim(new PersistedClaim(claim), new PersistedClaim(newClaim)), cancellationToken);
        }

        public Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveClaims(claims.Select(c => new PersistedClaim(c))), cancellationToken);
        }

        public async Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return await (await users.FindAsync(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value), cancellationToken: cancellationToken)).ToListAsync(cancellationToken);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.LockoutEndDate = lockoutEnd, cancellationToken);
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AccessFailedCount++, cancellationToken);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AccessFailedCount = 0, cancellationToken);
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.LockoutEnabled = enabled, cancellationToken);
        }

        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.PhoneNumber = phoneNumber, cancellationToken);
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.PhoneNumberConfirmed = confirmed, cancellationToken);
        }

        public Task SetTokenAsync(IdentityUser user, string loginProvider, string name, string value,
            CancellationToken cancellationToken)
        {
            return Task.Run(() => user.SetToken(new AuthToken(loginProvider, name, value)), cancellationToken);
        }

        public Task RemoveTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveToken(loginProvider, name), cancellationToken);
        }

        public Task<string> GetTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AuthTokens.SingleOrDefault(t => t.LoginProvider == loginProvider && t.Name == name)?.Value);
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.TwoFactorEnabled = enabled, cancellationToken);
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
    }
}
