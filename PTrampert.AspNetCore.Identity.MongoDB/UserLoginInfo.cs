using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class UserLoginInfo
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }

        public UserLoginInfo()
        {
        }

        public UserLoginInfo(Microsoft.AspNetCore.Identity.UserLoginInfo uli)
        {
            LoginProvider = uli.LoginProvider;
            ProviderKey = uli.ProviderKey;
            ProviderDisplayName = uli.ProviderDisplayName;
        }

        public Microsoft.AspNetCore.Identity.UserLoginInfo ToUserLoginInfo()
        {
            return new Microsoft.AspNetCore.Identity.UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }

        public override bool Equals(object obj)
        {
            var a = obj as UserLoginInfo;
            return a != null && Equals(a);
        }

        private bool Equals(UserLoginInfo other)
        {
            return LoginProvider == other.LoginProvider && ProviderDisplayName == other.ProviderDisplayName && ProviderKey == other.ProviderKey;
        }
    }
}
