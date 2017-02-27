using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class MongoUserStore : IUserStore<MongoIdentityUser>
    {
        private IMongoCollection<MongoIdentityUser> users;

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
    }
}
