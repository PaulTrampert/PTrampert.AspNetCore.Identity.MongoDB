using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class MongoUserLoginInfo
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }

        public MongoUserLoginInfo()
        {
        }

        public MongoUserLoginInfo(UserLoginInfo uli)
        {
            LoginProvider = uli.LoginProvider;
            ProviderKey = uli.ProviderKey;
            ProviderDisplayName = uli.ProviderDisplayName;
        }

        public UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }

        public override bool Equals(object obj)
        {
            var a = obj as MongoUserLoginInfo;
            return a != null && Equals(a);
        }

        private bool Equals(MongoUserLoginInfo other)
        {
            return LoginProvider == other.LoginProvider && ProviderDisplayName == other.ProviderDisplayName && ProviderKey == other.ProviderKey;
        }
    }
}
