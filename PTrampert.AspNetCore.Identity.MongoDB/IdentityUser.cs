using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

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

        public IdentityUser()
        {
            Logins = new List<PersistedUserLoginInfo>();
        }

        public void AddLogin(PersistedUserLoginInfo uli)
        {
            if (logins == null)
            {
                logins = new List<PersistedUserLoginInfo>();
            }
            logins.Add(uli);
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            Logins = Logins?.Except(Logins.Where(li => li.ProviderKey == providerKey && li.LoginProvider == loginProvider)).ToList();
        }
    }
}
