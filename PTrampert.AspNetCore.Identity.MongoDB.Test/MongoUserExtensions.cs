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
        public static bool PropertiesEqual(this IdentityUser self, IdentityUser other)
        {
            var properties = typeof(IdentityUser).GetProperties();
            var result = true;
            foreach (var prop in properties)
            {
                if (prop.Name == nameof(self.Logins))
                {
                    var selfLogins = self.Logins.OrderBy(li => li.ProviderKey);
                    var otherLogins = other.Logins.OrderBy(li => li.ProviderKey);
                    result = result && selfLogins.SequenceEqual(otherLogins);
                }
                else if (prop.Name == nameof(self.Claims))
                {
                    var selfClaims = self.Claims.OrderBy(c => c.Type);
                    var otherClaims = other.Claims.OrderBy(c => c.Type);
                    result = result && selfClaims.SequenceEqual(otherClaims);
                }
                else if (prop.Name == nameof(self.AuthTokens))
                {
                    var selfTokens = self.AuthTokens.OrderBy(t => t.LoginProvider);
                    var otherTokens = other.AuthTokens.OrderBy(t => t.LoginProvider);
                    result = result && selfTokens.SequenceEqual(otherTokens);
                }
                else if (prop.Name == nameof(self.Roles))
                {
                    var selfRoles = self.Roles.OrderBy(r => r);
                    var otherRoles = other.Roles.OrderBy(r => r);
                    result = result && selfRoles.SequenceEqual(otherRoles);
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
