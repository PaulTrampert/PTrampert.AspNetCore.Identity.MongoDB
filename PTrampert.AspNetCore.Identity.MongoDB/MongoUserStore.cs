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
        IUserLoginStore<MongoIdentityUser>,
        IUserRoleStore<MongoIdentityUser>,
        IUserClaimStore<MongoIdentityUser>,
        IUserPasswordStore<MongoIdentityUser>,
        IUserSecurityStampStore<MongoIdentityUser>,
        IUserEmailStore<MongoIdentityUser>,
        IUserLockoutStore<MongoIdentityUser>,
        IUserPhoneNumberStore<MongoIdentityUser>,
        IQueryableUserStore<MongoIdentityUser>,
        IUserAuthenticationTokenStore<MongoIdentityUser>,
        IUserTwoFactorStore<MongoIdentityUser>
    {
        private IMongoCollection<MongoIdentityUser> users;

        public IQueryable<MongoIdentityUser> Users => users.AsQueryable();

        public MongoUserStore(IMongoCollection<MongoIdentityUser> users)
        {
            this.users = users;
        }


        public async Task<IdentityResult> CreateAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            await users.InsertOneAsync(user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            await users.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            users = null;
        }

        public Task<MongoIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return users.Find(u => u.Id == userId).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<MongoIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return users.Find(u => u.NormalizedName == normalizedUserName).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedName);
        }

        public Task<string> GetUserIdAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(MongoIdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.NormalizedName = normalizedName, cancellationToken);
        }

        public Task SetUserNameAsync(MongoIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Name = userName, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            await users.FindOneAndReplaceAsync(u => u.Id == user.Id, user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(MongoIdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.PasswordHash = passwordHash, cancellationToken);
        }

        public Task<string> GetPasswordHashAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetEmailAsync(MongoIdentityUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Email = email, cancellationToken);
        }

        public Task<string> GetEmailAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(MongoIdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.EmailConfirmed = confirmed, cancellationToken);
        }

        public async Task<MongoIdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return (await users.FindAsync(u => u.NormalizedEmail == normalizedEmail, null, cancellationToken)).SingleOrDefault();
        }

        public Task<string> GetNormalizedEmailAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(MongoIdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.NormalizedEmail = normalizedEmail, cancellationToken);
        }

        public Task SetSecurityStampAsync(MongoIdentityUser user, string stamp, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.SecurityStamp = stamp, cancellationToken);
        }

        public Task<string> GetSecurityStampAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task AddLoginAsync(MongoIdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.AddLogin(login), cancellationToken);
        }

        public Task RemoveLoginAsync(MongoIdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.RemoveLogin(loginProvider, providerKey), cancellationToken);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MongoIdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(MongoIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(MongoIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(MongoIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<MongoIdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimsAsync(MongoIdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(MongoIdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(MongoIdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<MongoIdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(MongoIdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(MongoIdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberAsync(MongoIdentityUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(MongoIdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetTokenAsync(MongoIdentityUser user, string loginProvider, string name, string value,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTokenAsync(MongoIdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetTokenAsync(MongoIdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(MongoIdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetTwoFactorEnabledAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<string> IUserAuthenticationTokenStore<MongoIdentityUser>.GetTokenAsync(MongoIdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserTwoFactorStore<MongoIdentityUser>.GetTwoFactorEnabledAsync(MongoIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
