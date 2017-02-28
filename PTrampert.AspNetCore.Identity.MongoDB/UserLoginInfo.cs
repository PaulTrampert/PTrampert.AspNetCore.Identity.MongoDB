using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class PersistedUserLoginInfo : IEquatable<PersistedUserLoginInfo>, IEquatable<UserLoginInfo>
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }

        public PersistedUserLoginInfo()
        {
        }

        public PersistedUserLoginInfo(UserLoginInfo uli)
        {
            LoginProvider = uli.LoginProvider;
            ProviderKey = uli.ProviderKey;
            ProviderDisplayName = uli.ProviderDisplayName;
        }

        public UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }

        public override int GetHashCode()
        {
            return LoginProvider.GetHashCode() * 17 + ProviderKey.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is PersistedUserLoginInfo)
            {
                return Equals((PersistedUserLoginInfo)obj);
            }
            else if (obj is UserLoginInfo)
            {
                return Equals((UserLoginInfo)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(PersistedUserLoginInfo other)
        {
            return LoginProvider == other.LoginProvider && ProviderKey == other.ProviderKey;
        }

        public bool Equals(UserLoginInfo other)
        {
            return LoginProvider == other.LoginProvider && ProviderKey == other.ProviderKey;
        }
    }
}
