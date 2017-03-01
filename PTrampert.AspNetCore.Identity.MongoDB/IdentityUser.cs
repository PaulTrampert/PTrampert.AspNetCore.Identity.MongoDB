using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class IdentityUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string SecurityStamp { get; set; }
        public DateTimeOffset? LockoutEndDate { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }

        private List<PersistedUserLoginInfo> logins;
        [BsonIgnoreIfNull]
        public IEnumerable<PersistedUserLoginInfo> Logins
        {
            get
            {
                return logins ?? new List<PersistedUserLoginInfo>();
            }

            private set
            {
                logins = value.ToList();
            }
        }

        private List<PersistedClaim> claims;
        [BsonIgnoreIfNull]
        public IEnumerable<PersistedClaim> Claims
        {
            get
            {
                return claims ?? new List<PersistedClaim>();
            }
            private set
            {
                claims = value.ToList();
            }
        }

        public void AddClaims(IEnumerable<PersistedClaim> newClaims)
        {
            Claims = Claims.Union(newClaims);
        }

        public void AddClaim(PersistedClaim persistedClaim)
        {
            AddClaims(new[] { persistedClaim });
        }

        public void ReplaceClaim(PersistedClaim oldClaim, PersistedClaim newClaim)
        {
            if (claims.Remove(oldClaim))
            {
                claims.Add(newClaim);
            }
        }

        public void RemoveClaims(IEnumerable<PersistedClaim> claims)
        {
            Claims = Claims.Except(claims);
        }

        public IdentityUser()
        {
            Logins = new List<PersistedUserLoginInfo>();
        }

        public void AddLogins(IEnumerable<PersistedUserLoginInfo> ulis)
        {
            Logins = Logins.Union(ulis);
        }

        public void AddLogin(PersistedUserLoginInfo uli)
        {
            AddLogins(new[] { uli });
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            Logins = Logins?.Except(Logins.Where(li => li.ProviderKey == providerKey && li.LoginProvider == loginProvider)).ToList();
        }
    }
}
