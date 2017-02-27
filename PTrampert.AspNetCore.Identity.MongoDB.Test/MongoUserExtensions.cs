using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    public static class MongoUserExtensions
    {
        public static bool PropertiesEqual(this MongoIdentityUser self, MongoIdentityUser other)
        {
            var properties = typeof(MongoIdentityUser).GetProperties();
            var result = true;
            foreach (var prop in properties)
            {
                if (prop.Name == nameof(self.LoginInfo))
                {
                    var selfLogins = self.LoginInfo.OrderBy(li => li.ProviderKey);
                    var otherLogins = other.LoginInfo.OrderBy(li => li.ProviderKey);
                    result = result && selfLogins.SequenceEqual(otherLogins);
                }
                else
                {
                    result = result && prop.GetValue(self).Equals(prop.GetValue(other));
                }
            }
            return result;
        }
    }
}
