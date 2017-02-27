using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class MongoIdentityUser
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
        public List<MongoUserLoginInfo> LoginInfo { get; set; }

        public MongoIdentityUser()
        {
            LoginInfo = new List<MongoUserLoginInfo>();
        }

        public void AddLogin(UserLoginInfo uli)
        {
            if (LoginInfo == null)
            {
                LoginInfo = new List<MongoUserLoginInfo>();
            }
            LoginInfo.Add(new MongoUserLoginInfo(uli));
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            LoginInfo = LoginInfo?.Except(LoginInfo.Where(li => li.ProviderKey == providerKey && li.LoginProvider == loginProvider)).ToList();
        }
    }
}
