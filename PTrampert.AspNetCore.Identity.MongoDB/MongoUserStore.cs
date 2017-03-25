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
    public class MongoUserStore<T> :
        IUserLoginStore<T>,
        IUserClaimStore<T>,
        IUserPasswordStore<T>,
        IUserSecurityStampStore<T>,
        IUserEmailStore<T>,
        IUserLockoutStore<T>,
        IUserPhoneNumberStore<T>,
        IQueryableUserStore<T>,
        IUserAuthenticationTokenStore<T>,
        IUserTwoFactorStore<T>,
        IUserRoleStore<T>
        where T: IdentityUser
    {
        private IMongoCollection<T> users;

        public IQueryable<T> Users => users.AsQueryable();

        public MongoUserStore(IMongoCollection<T> users)
        {
            this.users = users;
        }


        public async Task<IdentityResult> CreateAsync(T user, CancellationToken cancellationToken)
        {
            await users.InsertOneAsync(user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(T user, CancellationToken cancellationToken)
        {
            await users.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            users = null;
        }

        public Task<T> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return users.Find(u => u.Id == userId).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<T> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return users.Find(u => u.NormalizedName == normalizedUserName).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedName);
        }

        public Task<string> GetUserIdAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(T user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.NormalizedName = normalizedName, cancellationToken);
        }

        public Task SetUserNameAsync(T user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Name = userName, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(T user, CancellationToken cancellationToken)
        {
            await users.FindOneAndReplaceAsync(u => u.Id == user.Id, user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(T user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.PasswordHash = passwordHash, cancellationToken);
        }

        public Task<string> GetPasswordHashAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetEmailAsync(T user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Email = email, cancellationToken);
        }

        public Task<string> GetEmailAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(T user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.EmailConfirmed = confirmed, cancellationToken);
        }

        public async Task<T> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return (await users.FindAsync(u => u.NormalizedEmail == normalizedEmail, null, cancellationToken)).SingleOrDefault();
        }

        public Task<string> GetNormalizedEmailAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(T user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.NormalizedEmail = normalizedEmail, cancellationToken);
        }

        public Task SetSecurityStampAsync(T user, string stamp, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.SecurityStamp = stamp, cancellationToken);
        }

        public Task<string> GetSecurityStampAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task AddLoginAsync(T user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AddLogin(new PersistedUserLoginInfo(login)), cancellationToken);
        }

        public Task RemoveLoginAsync(T user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveLogin(loginProvider, providerKey), cancellationToken);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Logins.Select(li => li.ToUserLoginInfo()).ToList() as IList<UserLoginInfo>);
        }

        public async Task<T> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return (await users.FindAsync(u => u.Logins.Any(li => li.LoginProvider == loginProvider && li.ProviderKey == providerKey), null, cancellationToken)).SingleOrDefault();
        }

        public Task<IList<Claim>> GetClaimsAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Claims.Select(c => new Claim(c.Type, c.Value)).ToList() as IList<Claim>);
        }

        public Task AddClaimsAsync(T user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AddClaims(claims.Select(c => new PersistedClaim(c))), cancellationToken);
        }

        public Task ReplaceClaimAsync(T user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.ReplaceClaim(new PersistedClaim(claim), new PersistedClaim(newClaim)), cancellationToken);
        }

        public Task RemoveClaimsAsync(T user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveClaims(claims.Select(c => new PersistedClaim(c))), cancellationToken);
        }

        public async Task<IList<T>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return await (await users.FindAsync(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value), cancellationToken: cancellationToken)).ToListAsync(cancellationToken);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(T user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.LockoutEndDate = lockoutEnd, cancellationToken);
        }

        public Task<int> IncrementAccessFailedCountAsync(T user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AccessFailedCount++, cancellationToken);
        }

        public Task ResetAccessFailedCountAsync(T user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AccessFailedCount = 0, cancellationToken);
        }

        public Task<int> GetAccessFailedCountAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(T user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.LockoutEnabled = enabled, cancellationToken);
        }

        public Task SetPhoneNumberAsync(T user, string phoneNumber, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.PhoneNumber = phoneNumber, cancellationToken);
        }

        public Task<string> GetPhoneNumberAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(T user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.PhoneNumberConfirmed = confirmed, cancellationToken);
        }

        public Task SetTokenAsync(T user, string loginProvider, string name, string value,
            CancellationToken cancellationToken)
        {
            return Task.Run(() => user.SetToken(new AuthToken(loginProvider, name, value)), cancellationToken);
        }

        public Task RemoveTokenAsync(T user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveToken(loginProvider, name), cancellationToken);
        }

        public Task<string> GetTokenAsync(T user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AuthTokens.SingleOrDefault(t => t.LoginProvider == loginProvider && t.Name == name)?.Value);
        }

        public Task SetTwoFactorEnabledAsync(T user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.TwoFactorEnabled = enabled, cancellationToken);
        }

        public Task<bool> GetTwoFactorEnabledAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task AddToRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AddToRole(roleName), cancellationToken);
        }

        public Task RemoveFromRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveFromRole(roleName), cancellationToken);
        }

        public Task<IList<string>> GetRolesAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles.ToList() as IList<string>);
        }

        public Task<bool> IsInRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Roles.Contains(roleName), cancellationToken);
        }

        public async Task<IList<T>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var result = await users.FindAsync(u => u.Roles.Contains(roleName), cancellationToken: cancellationToken);
            return await result.ToListAsync(cancellationToken);
        }
    }
}
