using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PTrampert.AspNetCore.Identity.MongoDB.Configuration;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class MongoRoleStore<TRole> : IRoleStore<TRole>
        where TRole : IdentityRole
    {
        private IMongoCollection<TRole> roles;

        public MongoRoleStore(IOptionsSnapshot<MongoRoleStoreOptions<TRole>> snapshot)
        {
            var opts = snapshot.Value;
            roles = opts.Roles;
        }

        public MongoRoleStore(IMongoCollection<TRole> roles)
        {
            this.roles = roles;
        }

        public void Dispose()
        {
            roles = null;
        }

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await roles.InsertOneAsync(role, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            await roles.FindOneAndReplaceAsync(r => r.Id == role.Id, role, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            await roles.DeleteOneAsync(r => r.Id == role.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() => role.Name = roleName, cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => role.NormalizedName = normalizedName, cancellationToken);
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return (await roles.FindAsync(r => r.Id == roleId, cancellationToken: cancellationToken)).SingleOrDefault();
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return (await roles.FindAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken)).SingleOrDefault();
        }
    }
}
