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

        [BsonIgnoreIfNull]
        public List<UserLoginInfo> Logins { get; set; }

        public IdentityUser()
        {
            Logins = new List<UserLoginInfo>();
        }

        public void AddLogin(Microsoft.AspNetCore.Identity.UserLoginInfo uli)
        {
            if (Logins == null)
            {
                Logins = new List<UserLoginInfo>();
            }
            Logins.Add(new UserLoginInfo(uli));
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            Logins = Logins?.Except(Logins.Where(li => li.ProviderKey == providerKey && li.LoginProvider == loginProvider)).ToList();
        }
    }
}
